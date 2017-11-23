using System;
using System.Collections.Generic;
using BlankDroid.Services;
using System.Threading.Tasks;
using BlankDroid.Models;

namespace BlankDroid
{
    public static class AnalysisContext
    {
        private static string _fullAudioPath;
        private static RecordingMetadata _metadata;
        private static FileService _fileService;

        public static List<Int16> samples = new List<short>();
        public static MainAdaptor adaptor;

        public static void UpdateContext(string baseLocation, string fileName)
        {
            _fileService = new FileService();
            _fullAudioPath = _fileService.GetFullPathToRecording(baseLocation, fileName);
            _metadata = _fileService.GetRecordingMetadata(baseLocation, fileName);
            UpdateSamples();
        }

        private static void UpdateSamples()
        {
            var audioSampleService = new AudioSampleService();

            Task.Run(() =>
            {
                samples = audioSampleService.GetSampleValues(_fullAudioPath, _metadata.AudioBitrate).Result;
            });
        }
    }
}