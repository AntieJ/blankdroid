using System;
using System.Collections.Generic;
using BlankDroid.Services;
using System.Threading.Tasks;
using BlankDroid.Models;

namespace BlankDroid
{
    public static class AnalysisContext
    {
        private static RecordingMetadata _metadata;
        private static FileService _fileService;

        public static List<Int16> samples = new List<short>();
        public static MainAdaptor adaptor;
        public static string BaseDirectory;
        public static string FileName;
        public static string FullAudioPath;

        public static void UpdateContext(string baseDirectory, string fileName)
        {
            _fileService = new FileService();

            FullAudioPath = _fileService.GetFullPathToRecording(baseDirectory, fileName);
            BaseDirectory = baseDirectory;
            FileName = fileName;
        }

        public static void UpdateSamples(string baseLocation, string fileName)
        {
            _fileService = new FileService();
            _metadata = _fileService.GetRecordingMetadata(baseLocation, fileName);
            UpdateSamples();
        }

        private static void UpdateSamples()
        {
            var audioSampleService = new AudioSampleService();

            Task.Run(() =>
            {
                samples = audioSampleService.GetSampleValues(FullAudioPath, _metadata.AudioBitrate).Result;
            });
        }
    }
}