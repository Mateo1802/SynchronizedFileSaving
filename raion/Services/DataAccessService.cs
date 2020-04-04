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
        private static readonly SemaphoreSlim semaphore_file = new SemaphoreSlim(1);
        //private static readonly SemaphoreSlim semaphore_stream = new SemaphoreSlim(1);

        public DataAccessService(IConfigurationRoot config)
        {
            _config = config;
        }

        public async Task SaveDataToFile(string message)
        {
            string targetFilePath = _config["FileOptions:Path"];
            string dateTimePattern = _config["FileOptions:DateFormat"];
            await semaphore_file.WaitAsync();
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
                semaphore_file.Release(1);
            }
        }

        public async Task SaveDataStreamWriter(string message)
        {
            string targetFilePath = _config["FileOptions:Path"];
            string dateTimePattern = _config["FileOptions:DateFormat"];
            await semaphore_file.WaitAsync();
            try
            {
                using (StreamWriter sw = new StreamWriter(targetFilePath))
                {
                    string appendText = DateTime.Now.ToString(dateTimePattern) + message + Environment.NewLine;
                    if (!File.Exists(targetFilePath))
                    {
                        string createText = DateTime.Now.ToString(dateTimePattern) + "File was created." + Environment.NewLine;
                        await sw.WriteLineAsync(appendText);
                    }
                    await sw.WriteLineAsync(appendText);
                    sw.Close();
                    sw.Dispose();
                }
            }
            finally
            {
                semaphore_file.Release(1);
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
