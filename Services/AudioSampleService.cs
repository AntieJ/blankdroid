using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlankDroid.Services
{
    public class AudioSampleService
    {
        public async Task<List<short>> GetSampleValues(string path)
        {
            var fileService = new FileService();
            var buffer = fileService.GetByteArrayFromFile(path);
            var sampleList = new List<Int16>();

            await Task.Run(() =>
            {
                for (int n = 0; n < buffer.Length; n += 2)
                {
                    Int16 sample = BitConverter.ToInt16(buffer, n);
                    sampleList.Add(sample);
                }
            });

            return sampleList;
        }
    }
}