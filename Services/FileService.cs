using System;
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

        public int GetFileLengthInSeconds(string filepath)
        {
            var byteArray = GetByteArrayFromFile(filepath);
            return (byteArray.Length / 2)/ConfigService.AudioFrequency;
        }
        
    }
}