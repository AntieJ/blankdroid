using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BlankDroid.Services
{
    class LoggingService
    {
        private bool WriteToDebug = true;
        private bool WriteToFile = true;
        private FileService _fileService;
        private string logDirectory = ConfigService.BaseDirectory;
        private string fileName;

        public LoggingService()
        {
            _fileService = new FileService();
            fileName = DateTime.UtcNow.ToString("dd-MM-yy")+"-Log";
        }

        public async Task Log(string message)
        {
            fileName = DateTime.UtcNow.ToString("dd-MM-yy") + "-Log";
            await Task.Run(() =>
            {
                var messageToLog = $"{DateTime.Now.ToLongTimeString()} - {message} { Environment.NewLine}";

                if (WriteToDebug)
                {
                    Debug.WriteLine(messageToLog);
                }

                if (WriteToFile)
                {
                    _fileService.SaveOrAddToTextFile(logDirectory, fileName, messageToLog);
                }
            });   
        }
    }
}