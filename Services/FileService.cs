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

        public bool TryDeleteByFileName(string baseDirectory, string fileName)
        {
            try
            {
                var audioPath = GetFullPathToRecording(baseDirectory, fileName);
                var metadataPath = GetFullPathToMetadata(baseDirectory, fileName);
                var displayLinesPath = GetFullPathToDisplayLinesFile(baseDirectory, fileName);
                File.Delete(audioPath);
                File.Delete(metadataPath);
                File.Delete(displayLinesPath);

                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public long GetFileSizeInKB(string baseDirectory, string fileName)
        {
            var metadata = GetRecordingMetadata(baseDirectory, fileName);
            if (metadata.FileSizeKb != -1 && metadata.FileSizeKb != 0)
            {
                return metadata.FileSizeKb;
            }
            var fullRecordingPath = GetFullPathToRecording(baseDirectory, fileName);
            metadata.FileSizeKb = GetFileSizeInKBDirectlyFromFile(fullRecordingPath);
            UpdateMetadataFile(metadata, baseDirectory, fileName);

            return metadata.FileSizeKb;
        }

        private long GetFileSizeInKBDirectlyFromFile(string filepath)
        {
            return new FileInfo(filepath).Length / 1000;
        }

        public int GetAudioFileLengthInSeconds(string baseDirectory, string fileName)
        {
            var metadata = GetRecordingMetadata(baseDirectory, fileName);
            if (metadata.FileLengthSeconds != -1 && metadata.FileLengthSeconds != 0)
            {
                return metadata.FileLengthSeconds;
            }
            var fullRecordingPath = GetFullPathToRecording(baseDirectory, fileName);
            metadata.FileLengthSeconds = GetAudioFileLengthInSecondsDirectlyFromFile(fullRecordingPath);
            UpdateMetadataFile(metadata, baseDirectory, fileName);

            return metadata.FileLengthSeconds;
        }

        private int GetAudioFileLengthInSecondsDirectlyFromFile(string filepath)
        {
            var byteArray = GetByteArrayFromFile(filepath);
            return (byteArray.Length / 2) / ConfigService.AudioFrequency;
        }

        public void SaveNewMetadataFile(string fileName, 
            DateTime startedAt, 
            int frequency, 
            Android.Media.Encoding bitrate, 
            Dictionary<string, bool> factors, 
            string note,
            int fileSizeKb=-1,
            int fileLengthSeconds=-1)
        {
            using (var fs = new FileStream(GetFullPathToNewMetadata(fileName), FileMode.Create, FileAccess.Write))
            {

                string dataasstring = JsonConvert.SerializeObject(new RecordingMetadata()
                {
                    AudioFrequency = frequency,
                    AudioBitrate = bitrate,
                    Factors = factors,
                    Note = note,
                    StartedAt = startedAt,
                    FileSizeKb = fileSizeKb,
                    FileLengthSeconds = fileLengthSeconds
                });
                byte[] info = new UTF8Encoding(true).GetBytes(dataasstring);
                fs.Write(info, 0, info.Length);
                fs.Close();
            }
        }

        public void UpdateMetadataFile(RecordingMetadata metadata, string baseDirectory, string fileName)
        {
            using (var fs = new FileStream(GetFullPathToMetadata(baseDirectory, fileName), FileMode.Open, FileAccess.Write))
            {
                string dataasstring = JsonConvert.SerializeObject(metadata);
                byte[] info = new UTF8Encoding(true).GetBytes(dataasstring);
                fs.Write(info, 0, info.Length);
                fs.Close();
            }
        }

        //public void UpdateMetadataFile(string fileName, int frequency, Android.Media.Encoding bitrate, Dictionary<string, bool> factors, string note)
        //{
        //    //get file
        //    //overwrite variables
        //    //save file
        //    File.WriteAllText(GetFullPathToNewMetadata(fileName), JsonConvert.SerializeObject(new RecordingMetadata()
        //    {
        //        AudioFrequency = frequency,
        //        AudioBitrate = bitrate,
        //        Factors = factors,
        //        Note = note
        //    }));
        //}

        public string GetFullPathToNewMetadata(string fileName)
        {
            return GetFullPathToMetadata(ConfigService.BaseDirectory, fileName);
        }

        public string GetFullPathToNewRecording(string fileName)
        {
            return GetFullPathToRecording(ConfigService.BaseDirectory, fileName);
        }

        public string GetFullPathToNewDisplayLinesFile(string fileName)
        {
            return GetFullPathToDisplayLinesFile(ConfigService.BaseDirectory, fileName);
        }

        public string GetFullPathToMetadata(string baseDirectory, string fileName)
        {
            return $"{baseDirectory}{fileName}{ConfigService.MetadataFileExtension}";
        }

        public string GetFullPathToRecording(string baseDirectory, string fileName)
        {
            return $"{baseDirectory}{fileName}{ConfigService.AudioFileExtension}";
        }

        public string GetFullPathToDisplayLinesFile(string baseDirectory, string fileName)
        {
            return $"{baseDirectory}{fileName}{ConfigService.DisplayLinesFileExtension}";
        }

        public RecordingMetadata GetRecordingMetadata(string basePath, string fileName)
        {
            try
            {
                var metadataString = File.ReadAllText($"{basePath}{fileName}{ConfigService.MetadataFileExtension}");
                return JsonConvert.DeserializeObject<RecordingMetadata>(metadataString);
            }
            catch(Exception ex)
            {
                _loggingService.Log($"Failed to get metadata for {fileName}: {ex.Message}");
                return new RecordingMetadata();
            }

        }

        public string GenerateFileNameWithoutExtension()
        {
            return ConfigService.BaseName +
                DateTime.UtcNow.ToString("dd-MM-yy-HH:mm:ss");
        }

        public bool SaveProcessedDisplayLines(SimpleLine[] lineArray, string fileName)
        {
            var jsonArray = JsonConvert.SerializeObject(lineArray);
            //File.WriteAllText(GetFullPathToNewDisplayLinesFile(fileName), jsonArray);

            using (var fs = new FileStream(GetFullPathToNewDisplayLinesFile(fileName), FileMode.Create, FileAccess.Write))
            {

                string dataasstring = jsonArray; //your data
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