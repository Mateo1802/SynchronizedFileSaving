using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace raion.Services
{
    public class DataAccessService : IDataAccessService
    {
        private readonly IConfigurationRoot _config;
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public DataAccessService(IConfigurationRoot config)
        {
            _config = config;
        }

        public async Task SaveDataToFile(string message)
        {
            string targetFilePath = _config["FileOptions:Path"];
            string dateTimePattern = _config["FileOptions:DateFormat"];
            await semaphore.WaitAsync();
            try
            {
                if (!File.Exists(targetFilePath))
                {
                    string createText = DateTime.Now.ToString(dateTimePattern) + "File was created." + Environment.NewLine;
                    await File.WriteAllTextAsync(targetFilePath, createText);
                }
                string appendText = DateTime.Now.ToString(dateTimePattern) + message + Environment.NewLine;
                await File.AppendAllTextAsync(targetFilePath, appendText);
            }
            finally
            {
                semaphore.Release(1);
            }
        }

        public async Task<string> ReadData()
        {
            string filePath = _config["FileOptions:Path"];
            string data = await File.ReadAllTextAsync(filePath);
            return data;
        }
    }
}
