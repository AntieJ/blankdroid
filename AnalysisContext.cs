using System;
using System.Collections.Generic;
using BlankDroid.Services;
using System.Threading.Tasks;

namespace BlankDroid
{
    public static class AnalysisContext
    {
        public static string _path;
        public static List<Int16> samples = new List<short>();
        public static MainAdaptor adaptor;

        public static void UpdateContext(string path)
        {
            _path = path;
            UpdateSamples();
        }

        private static void UpdateSamples()
        {
            var audioSampleService = new AudioSampleService();

            Task.Run(() =>
            {
                samples = audioSampleService.GetSampleValues(_path).Result;
            });
        }
    }
}