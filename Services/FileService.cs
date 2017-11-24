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
                    values.Add(value.Replace(baseDirectory, "").Replace(fileExtension, ""));
                }
            }
            return values;
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

        public long GetFileSizeInKB(string filepath)
        {
            return new FileInfo(filepath).Length / 1000;
        }

        public int GetAudioFileLengthInSeconds(string filepath)
        {
            var byteArray = GetByteArrayFromFile(filepath);
            return (byteArray.Length / 2) / ConfigService.AudioFrequency;
        }

        public void SaveNewMetadataFile(string fileName, int frequency, Android.Media.Encoding bitrate)
        {
            File.WriteAllText(GetFullPathToNewMetadata(fileName), JsonConvert.SerializeObject(new RecordingMetadata()
            {
                AudioFrequency = frequency,
                AudioBitrate = bitrate
            }));
        }

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
            catch
            {
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
            File.WriteAllText(GetFullPathToNewDisplayLinesFile(fileName), jsonArray);
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

        public void SaveOrAddToTextFile(string directory, string fileName, string content)
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