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
        private static AudioFileService _audioFileService;
        private static MetadataService _metadataService;
        public static List<Int16> samples = new List<short>();
        public static MainAdaptor adaptor;
        public static string BaseDirectory;
        public static string FileName;
        public static string FullAudioPath;

        public static void UpdateContext(string baseDirectory, string fileName)
        {
            _audioFileService = new AudioFileService();

            FullAudioPath = _audioFileService.GetFullPathToRecording(baseDirectory, fileName);
            BaseDirectory = baseDirectory;
            FileName = fileName;
        }

        public static void UpdateSamples(string baseLocation, string fileName)
        {
            _metadataService = new MetadataService();
            _metadata = _metadataService.GetRecordingMetadata(baseLocation, fileName);
            UpdateSamples();
        }

        private static void UpdateSamples()
        {
            var waveFormService = new WaveformService();

            Task.Run(() =>
            {
                samples = waveFormService.GetSampleValues(FullAudioPath, _metadata.AudioBitrate).Result;
            });
        }
    }
}