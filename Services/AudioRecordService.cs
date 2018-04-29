using System;
using Android.Media;
using System.Threading.Tasks;
using System.IO;

namespace BlankDroid.Services
{
    public static class AudioRecordService
    {
        static AudioRecord _audioRecord;
        static Byte[] _audioBuffer;
        static bool _endRecording;
        static FileService _fileService = new FileService();

        //public AudioRecordService()
        //{
        //    _fileService = new FileService();
        //}

        public static async Task Start(string filename)
        {
            _endRecording = false;
            _audioBuffer = new Byte[100000];
            _audioRecord = new AudioRecord(
                AudioSource.Mic,
                ConfigService.AudioFrequency,
                ChannelIn.Mono,
                ConfigService.AudioBitrate,
                _audioBuffer.Length
            );

            _audioRecord.StartRecording();
            await ReadAudioAsync(filename);
        }

        public static void Stop()
        {
            _endRecording = true;

            //Thread.Sleep(500); // Give it time to drop out.
        }

        private static async Task ReadAudioAsync(string filename)
        {
            if (!Directory.Exists(ConfigService.BaseDirectory))
            {
                Directory.CreateDirectory(ConfigService.BaseDirectory);
            }


            using (var fileStream = new FileStream(_fileService.GetFullPathToNewRecording(filename), FileMode.Create, FileAccess.Write))
            {
                while (true)
                {
                    if (_endRecording)
                    {
                        _endRecording = false;
                        fileStream.Close();
                        fileStream.Dispose();
                        break;
                    }

                    try
                    {
                        int numBytes = await _audioRecord.ReadAsync(_audioBuffer, 0, _audioBuffer.Length);
                        await fileStream.WriteAsync(_audioBuffer, 0, numBytes);
                    }
                    catch (Exception ex)
                    {
                        Console.Out.WriteLine(ex.Message);
                        break;
                    }
                }

                fileStream.Close();
                fileStream.Dispose();
            }
            _audioRecord.Stop();
            _audioRecord.Release();
            
        }
    }
}