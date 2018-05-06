using System;
using System.Collections.Generic;
using System.Text;
using BlankDroid.Models;
using System.IO;
using Newtonsoft.Json;

namespace BlankDroid.Services
{
    public class MetadataService
    {
        private FileService _fileService;
        private LoggingService _loggingService;

        public MetadataService()
        {
            _fileService = new FileService();
            _loggingService = new LoggingService();
        }

        public RecordingMetadata GetRecordingMetadata(string basePath, string fileName)
        {
            try
            {
                var metadataString = File.ReadAllText($"{basePath}{fileName}{ConfigService.MetadataFileExtension}");
                return JsonConvert.DeserializeObject<RecordingMetadata>(metadataString);
            }
            catch (Exception ex)
            {
                _loggingService.Log($"Failed to get metadata for {fileName}: {ex.Message}");
                return new RecordingMetadata();
            }
        }

        public void SaveNewMetadataFile(string fileName,
            DateTime startedAt,
            int frequency,
            Android.Media.Encoding bitrate,
            Dictionary<string, bool> factors,
            string note,
            int fileSizeKb = -1,
            int fileLengthSeconds = -1)
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

        public string GetFullPathToNewMetadata(string fileName)
        {
            return GetFullPathToMetadata(ConfigService.BaseDirectory, fileName);
        }

        public string GetFullPathToMetadata(string baseDirectory, string fileName)
        {
            return _fileService.GetFullPath(baseDirectory, fileName, ConfigService.MetadataFileExtension);
        }
    }
}