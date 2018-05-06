using System.Collections.Generic;
using System.Linq;
using BlankDroid.Models;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace BlankDroid.Services
{
    public class WaveformService
    {
        private LoggingService _loggingService;
        private Dictionary<string, int> retryLog = new Dictionary<string, int>();
        private int retryMaxCount=5;
        private MetadataService _metadataService;
        private AudioFileService _audioFileService;
        private FileService _fileService;

        public WaveformService()
        {
            _loggingService = new LoggingService();
            _metadataService = new MetadataService();
            _audioFileService = new AudioFileService();
            _fileService = new FileService();
        }

        public async Task ProcessAndSaveDisplayLines(string baseDirectory, string fileName)
        {
            try
            {
                if (_fileService.ProcessedDisplayLinesFileExists(baseDirectory, fileName))
                {
                    //nothing to do
                    return;
                }
                else
                {
                    var audioPath = _audioFileService.GetFullPathToRecording(baseDirectory, fileName);
                    var metadata = _metadataService.GetRecordingMetadata(baseDirectory, fileName);
                    var samples = await GetSampleValues(audioPath, metadata.AudioBitrate);

                    var displayLines = GetLinesFromSamples(ConfigService.PixelWidth, ConfigService.YAxis,
                        ConfigService.ChartHeight, samples);
                    _fileService.SaveProcessedDisplayLines(displayLines, fileName);
                    await _loggingService.LogAsync($"Processed file succesfully: {fileName} ");

                    
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error processing file! {fileName} {ex.Message}");
                
                if(!retryLog.ContainsKey(fileName) || retryLog[fileName] < retryMaxCount)
                {
                    
                    await RetryProcessAndSaveDisplayLines(baseDirectory, fileName);
                }
                else
                {
                    await _loggingService.LogAsync($"Giving up processing. {fileName} {ex.Message}");
                }
            }
        }


        public SimpleLine[] GetLinesFromSamples(int screenWidth, int yAxis, int chartHeight, List<short> samples)
        {
            if(samples == null || samples.Count < 1)
            {
                return new SimpleLine[] { };
            }

            var stepSize = (int)Math.Ceiling(samples.Count / (decimal)5000);
            var smallBufferArray = GetFilteredArray(samples, stepSize);
            var sampleLines = new SimpleLine[smallBufferArray.Length];
            var arrayMax = (float)smallBufferArray.Max();
            var arrayLength = smallBufferArray.Length;

            for (var i = 0; i < (arrayLength); i++)
            {
                float xAnchor = ((float)i / (float)arrayLength) * screenWidth;
                float lineHeightPercent = (float)smallBufferArray[i] / arrayMax;
                float lineHeightScaled = (float)lineHeightPercent * (float)chartHeight;
                var lineHeight = (float)yAxis - (float)lineHeightScaled;

                sampleLines[i] = new SimpleLine
                {
                    Start = new System.Drawing.Point((int)xAnchor, yAxis),
                    End = new System.Drawing.Point((int)xAnchor, (int)lineHeight)
                };

                if (i % 10000 == 0)
                {
                    System.Diagnostics.Debug.WriteLine("linesfromsamples, i is: " + i);

                }

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

        private async Task RetryProcessAndSaveDisplayLines(string baseDirectory, string fileName)
        {

            Thread.Sleep(2000);
            if (retryLog.ContainsKey(fileName))
            {
                retryLog[fileName] = (int)retryLog[fileName] + 1;
            }
            else
            {
                retryLog[fileName] = 1;
            }
            await _loggingService.LogAsync($"Retrying {fileName} Retry count: {retryLog[fileName]}");

            await ProcessAndSaveDisplayLines(baseDirectory, fileName);
        }

        private async Task<List<short>> GetSampleValues16Bit(string path)
        {
            //var fileService = new FileService();
            var buffer = _fileService.GetByteArrayFromFile(path);
            var sampleList = new List<Int16>();

            await Task.Run(() =>
            {
                for (int n = 0; n < buffer.Length; n += 2)
                {
                    Int16 sample = BitConverter.ToInt16(buffer, n);
                    sampleList.Add(sample);
                    if (n % 1000000 == 0)
                    {
                        System.Diagnostics.Debug.WriteLine("samples16bit, n is: " + n);

                    }
                }
            });
            
            return sampleList;
        }

        //todo: Thread never completes.
        private async Task<List<short>> GetSampleValues8Bit(string path)
        {
            //var fileService = new FileService();
            var buffer = _fileService.GetByteArrayFromFile(path);
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

            if(samples==null || samples.Count < 1)
            {
                return new short[] { };
            }

            var filtered = samples.Where((x, i) => i % stepSize == 0).ToList();

            //remove highest and lowest X samples ->make rendering look better, removes pops
            for(var i=0; i < 10; i++)
            {
                //var maxIndex = samples.IndexOf(samples.Max());
                filtered.Remove(filtered.Max());
                filtered.Remove(filtered.Min());
            }
            

            return filtered.ToArray();
        }

        private SimpleLine[] GetSubsetOfLines(SimpleLine[] allLines)
        {
            //on large records, we dont need to display ALL the lines
            var spacer = (int)Math.Ceiling(allLines.Length / (decimal)1500);
            var filteredLines = new List<SimpleLine>();
            for (var i = 0; i < (allLines.Length); i = i + spacer)
            {
                filteredLines.Add(allLines[i]);
            }
            return filteredLines.ToArray();
        }
    }
}