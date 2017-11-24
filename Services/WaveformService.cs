using System.Collections.Generic;
using System.Linq;
using BlankDroid.Models;
using System.Threading.Tasks;
using System;

namespace BlankDroid.Services
{
    public class WaveformService
    {
        private int stepSize = 100;
        private FileService _fileService;

        public WaveformService()
        {
            _fileService = new FileService();
        }

        public async Task ProcessAndSaveDisplayLines(string baseDirectory, string fileName)
        {
            var audioPath = _fileService.GetFullPathToRecording(baseDirectory, fileName);
            var metadata = _fileService.GetRecordingMetadata(baseDirectory, fileName);
            var samples = await GetSampleValues(audioPath, metadata.AudioBitrate);

            if (_fileService.ProcessedDisplayLinesFileExists(baseDirectory, fileName))
            {
                //nothing to do
                return;
            }
            else
            {
                var displayLines = GetLinesFromSamples(ConfigService.PixelWidth, ConfigService.YAxis,
                    ConfigService.ChartHeight, samples);
                _fileService.SaveProcessedDisplayLines(displayLines, fileName);
            }
        }


        public SimpleLine[] GetLinesFromSamples(int screenWidth, int yAxis, int chartHeight, List<short> samples)
        {
            var smallBufferArray = GetFilteredArray(samples, stepSize);
            var sampleLines = new SimpleLine[smallBufferArray.Length];

            for (var i = 0; i < (smallBufferArray.Length); i++)
            {
                float xAnchor = ((float)i / (float)smallBufferArray.Length) * screenWidth;
                float lineHeightPercent = (float)smallBufferArray[i] / (float)smallBufferArray.Max();
                float lineHeightScaled = (float)lineHeightPercent * (float)chartHeight;
                var lineHeight = (float)yAxis - (float)lineHeightScaled;

                sampleLines[i] = new SimpleLine
                {
                    Start = new System.Drawing.Point((int)xAnchor, yAxis),
                    End = new System.Drawing.Point((int)xAnchor, (int)lineHeight)
                };
            }

            return sampleLines;
        }

        public SimpleLine GetBaseLine(int screenWidth, int yAxis)
        {
            return new SimpleLine
            {
                Start = new System.Drawing.Point(0, yAxis),
                End = new System.Drawing.Point(screenWidth, yAxis)
            };
        }

        public async Task<List<short>> GetSampleValues(string path, Android.Media.Encoding bitrate)
        {
            if (bitrate == Android.Media.Encoding.Pcm8bit)
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
                for (int n = 0; n < buffer.Length; n++)
                {
                    var thing = buffer[n];
                    var sample = (Int16)Convert.ToSByte((byte)buffer[n]); //n++
                    //Int16 sample = BitConverter.ToInt16(buffer, n); //+=2
                    sampleList.Add(sample);
                }

            });

            return sampleList;

        }

        private short[] GetFilteredArray(List<short> samples, int stepSize)
        {
            /*Creates a smaller array taking every X sample
            Do this because we don't need that much resolution when displaying
            Consider lowering the sample frequency of recording
            This could be done on file read*/

            return samples.Where((x, i) => i % stepSize == 0).ToArray();
        }
    }
}