using System;
using System.Collections.Generic;

namespace BlankDroid.Models
{
    public class RecordingMetadata
    {
        public string Note { get; set; }
        public Dictionary<string, bool> Factors { get; set; }
        public int AudioFrequency { get; set; }
        public Android.Media.Encoding AudioBitrate { get; set; }    
        public DateTime StartedAt { get; set; }
        public long FileSizeKb { get; set; }
        public int FileLengthSeconds { get; set; }
    }
}