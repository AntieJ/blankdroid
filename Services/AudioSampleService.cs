using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BlankDroid.Services
{
    public class AudioSampleService
    {
        public async Task<List<short>> GetSampleValues(string path, Android.Media.Encoding bitrate)
        {
            if(bitrate == Android.Media.Encoding.Pcm8bit)
            {
                return await GetSampleValues8Bit(path);
            }
            else
            {

                //default to 16 bit
                return await GetSampleValues16Bit(path);

            }
        }

        private async Task<List<short>> GetSampleValues16Bit(string path)
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

        //todo: Thread never completes.
        private async Task<List<short>> GetSampleValues8Bit(string path)
        {
            var fileService = new FileService();
            var buffer = fileService.GetByteArrayFromFile(path);
            var sampleList = new List<Int16>();
            await Task.Run(() =>
            {
                for (int n = 0; n < buffer.Length; n ++)
                {
                    var thing=buffer[n];
                    var sample = (Int16)Convert.ToSByte((byte)buffer[n]); //n++
                    //Int16 sample = BitConverter.ToInt16(buffer, n); //+=2
                    sampleList.Add(sample);
                }

            });

            return sampleList;

        }
    }
}