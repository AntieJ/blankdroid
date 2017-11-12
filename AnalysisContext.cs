using System;
using System.Collections.Generic;
using BlankDroid.Services;
using System.Threading.Tasks;

namespace BlankDroid
{
    public static class AnalysisContext
    {
        public static string _fullAudioPath;
        public static List<Int16> samples = new List<short>();
        public static MainAdaptor adaptor;

        public static void UpdateContext(string path)
        {
            _fullAudioPath = path;
            UpdateSamples();
        }

        private static void UpdateSamples()
        {
            var audioSampleService = new AudioSampleService();

            Task.Run(() =>
            {
                samples = audioSampleService.GetSampleValues(_fullAudioPath).Result;
            });
        }
    }
}