using BlankDroid.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BlankDroid.Services
{
    public class FileService
    {
        public byte[] GetByteArrayFromFile(string filePath)
        {
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var binaryReader = new BinaryReader(fileStream);
            var buffer = binaryReader.ReadBytes((Int32)new FileInfo(filePath).Length);

            fileStream.Close();
            fileStream.Dispose();
            binaryReader.Close();
            binaryReader.Dispose();

            return buffer;
        }

        public List<string> GetFilesFromDirectory(string baseDirectory, string fileExtension)
        {
            var values = new List<string>();
            foreach (var value in Directory.GetFiles(baseDirectory))
            {
                if (value.EndsWith(fileExtension))
                {
                    values.Add(value.Replace(baseDirectory, ""));
                }
            }
            return values;
        }

        public bool TryDelete(string filepath)
        {
            try
            {
                File.Delete(filepath);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public long GetFileSizeInKB(string filepath)
        {
            return new FileInfo(filepath).Length / 1000;
        }

        public int GetAudioFileLengthInSeconds(string filepath)
        {
            var byteArray = GetByteArrayFromFile(filepath);
            return (byteArray.Length / 2)/ConfigService.AudioFrequency;
        }

        public void SaveNewMetadataFile(int frequency, Android.Media.Encoding bitrate)
        {
            File.WriteAllText(GetFullPathToNewMetadata(), JsonConvert.SerializeObject(new RecordingMetadata()
            {
                AudioFrequency = frequency,
                AudioBitrate = bitrate
            }));
        }

        public string GetFullPathToNewMetadata()
        {
            return $"{GetNewFileName()}{ConfigService.MetadataFileExtension}";
        }

        public string GetFullPathToNewRecording()
        {
            return $"{GetNewFileName()}{ConfigService.AudioFileExtension}";
        }

        public RecordingMetadata GetRecordingMetadata(string basePath, string fileName)
        {
            try
            {
                var metadataString = File.ReadAllText($"{basePath}{fileName}{ConfigService.MetadataFileExtension}");
                return JsonConvert.DeserializeObject<RecordingMetadata>(metadataString);
            }
            catch
            {
                return new RecordingMetadata();
            }
            
        }

        private string GetNewFileName()
        {
            return ConfigService.BaseDirectory +
                ConfigService.BaseName +
                DateTime.UtcNow.ToString("dd-MM-yy-HH:mm:ss");
        }

    }
}