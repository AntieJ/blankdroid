﻿namespace BlankDroid.Services
{
    public static class ConfigService
    {
        public static string BaseDirectory = "/sdcard/recordings/";
        public static string BaseName = "Rec-";
        public static string FileExtension = ".pcm";
        public static int AudioFrequency = 11025;
        public static Android.Media.Encoding AudioBitrate = Android.Media.Encoding.Pcm16bit;
        public static string DirectoryToAnalyse = "";
    }
}