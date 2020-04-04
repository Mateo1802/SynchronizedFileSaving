using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using raion.Models;
using raion.Services;


namespace raion.Controllers
{
    [Route("api/[controller]")]
    public class DataController : Controller
    {
        private readonly IDataAccessService _dataAccessService;
        public DataController(IDataAccessService dataAccessService)
        {
            _dataAccessService = dataAccessService;
        }

        [HttpGet("savedata")]
        public async Task<SaveResponse> SaveData([FromQuery] string text)
        {
            try
            {
                await _dataAccessService.SaveDataToFile(text);
                return new SaveResponse(true, "Text added.");
            }
            catch (Exception e)
            {
                return new SaveResponse(false, $"Text adding error. Exception catched: {e.ToString()}");
            }
        }

        [HttpGet("paralleltest")]
        public async Task Get()
        {
            int[] ids = new[] { 1, 2, 3, 4, 5 };
            Parallel.ForEach(ids, i => _dataAccessService.SaveDataToFile(i.ToString()).Wait());
            await Task.CompletedTask;
        }
    }
}
