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
            var buffer = binaryReader.ReadBytes((Int32)new FileInfo(ConfigService.PathToRecording).Length);

            fileStream.Close();
            fileStream.Dispose();
            binaryReader.Close();
            binaryReader.Dispose();

            return buffer;
        }
    }
}