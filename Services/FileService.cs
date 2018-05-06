using BlankDroid.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlankDroid.Services
{
    public class FileService
    {
        private LoggingService _loggingService;

        public FileService()
        {
            _loggingService = new LoggingService();
        }

        public byte[] GetByteArrayFromFile(string filePath)
        {
            byte[] buffer;
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {

                var binaryReader = new BinaryReader(fileStream);
                buffer = binaryReader.ReadBytes((Int32)new FileInfo(filePath).Length);
                
                fileStream.Close();
                fileStream.Dispose();
                binaryReader.Close();
                binaryReader.Dispose();
            }

            return buffer;
        }

        public List<string> GetFilesFromDirectory(string baseDirectory, string fileExtension)
        {
            var values = new List<string>();

            if (!DirectoryExists(baseDirectory))
            {
                return values;
            }

            foreach (var value in Directory.GetFiles(baseDirectory))
            {
                if (value.EndsWith(fileExtension))
                {
                    values.Add(value.Replace(baseDirectory, "").Replace(fileExtension, ""));
                }
            }
            return values;
        }

        public bool DirectoryExists(string directory)
        {
            return Directory.Exists(directory);            
        }     

        public long GetFileSizeInKBDirectlyFromFile(string filepath)
        {
            return new FileInfo(filepath).Length / 1000;
        }      

        public void DeleteFile(string fullPath)
        {
            File.Delete(fullPath);
        }

        

        public string GetFullPathToNewDisplayLinesFile(string fileName)
        {
            return GetFullPathToDisplayLinesFile(ConfigService.BaseDirectory, fileName);
        }

        

        public string GetFullPathToDisplayLinesFile(string baseDirectory, string fileName)
        {
            return $"{baseDirectory}{fileName}{ConfigService.DisplayLinesFileExtension}";
        }

        public string GetFullPath(string baseDirectory, string fileName, string extension)
        {
            return $"{baseDirectory}{fileName}{extension}";
        }

        
        public string GenerateFileNameWithoutExtension()
        {
            return ConfigService.BaseName +
                DateTime.Now.ToString("dd-MM-yy-HH:mm:ss");
        }

        public bool SaveProcessedDisplayLines(SimpleLine[] lineArray, string fileName)
        {
            var jsonArray = JsonConvert.SerializeObject(lineArray);

            using (var fs = new FileStream(GetFullPathToNewDisplayLinesFile(fileName), FileMode.Create, FileAccess.Write))
            {

                string dataasstring = jsonArray;
                byte[] info = new UTF8Encoding(true).GetBytes(dataasstring);
                fs.Write(info, 0, info.Length);
                fs.Close();
            }

            return true;
        }

        public SimpleLine[] GetProcessedDisplayLines(string baseDirectory, string fileName)
        {
            if (!ProcessedDisplayLinesFileExists(baseDirectory, fileName))
            {
                throw new Exception("Display lines file does not exist");
            }

            var linesFileString = File.ReadAllText(GetFullPathToDisplayLinesFile(baseDirectory, fileName));
            return JsonConvert.DeserializeObject<SimpleLine[]>(linesFileString);
        }

        public bool ProcessedDisplayLinesFileExists(string baseDirectory, string fileName)
        {
            return File.Exists(GetFullPathToDisplayLinesFile(baseDirectory, fileName));
        }        
    }
}