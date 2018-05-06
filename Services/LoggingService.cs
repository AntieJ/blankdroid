using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace BlankDroid.Services
{
    class LoggingService
    {
        private bool WriteToDebug = false;
        private bool WriteToFile = true;
        private string logDirectory = ConfigService.BaseDirectory;
        private string fileName;

        public void Log(string message)
        {
            Task.Run(async () =>
            {
                await LogAsync(message);
            });
        }

        public async Task LogAsync(string message)
        {
            fileName = DateTime.Now.ToString("dd-MM-yy") + "-Log";
            await Task.Run(() =>
            {
                var messageToLog = $"{DateTime.Now.ToLongTimeString()} - {message} { Environment.NewLine}";

                if (WriteToDebug)
                {
                    Debug.WriteLine(messageToLog);
                }

                if (WriteToFile)
                {
                    SaveOrAddToTextFile(logDirectory, fileName, messageToLog);
                }
            });   
        }

        //Can't use FileService, otherwise circular reference
        private void SaveOrAddToTextFile(string directory, string fileName, string content)
        {
            var fullPath = $"{directory}{fileName}.txt";

            if (File.Exists(fullPath))
            {
                content = content + Environment.NewLine + File.ReadAllText(fullPath);
            }

            File.WriteAllText(fullPath, content);
        }
    }
}