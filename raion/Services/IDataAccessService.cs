using System;
using System.Threading.Tasks;

namespace raion.Services
{
    public interface IDataAccessService
    {
        Task SaveDataToFile(string message);
    }
}
