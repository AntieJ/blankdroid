using System;
using Android.Media;
using System.Threading.Tasks;
using System.IO;

namespace BlankDroid.Services
{
    public class AudioRecordService
    {
        AudioRecord _audioRecord;
        Byte[] _audioBuffer;
        bool _endRecording;
        FileService _fileService;

        public AudioRecordService()
        {
            _fileService = new FileService();
        }

        public async Task Start()
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
            await ReadAudioAsync();
        }

        public void Stop()
        {
            _endRecording = true;
            //Thread.Sleep(500); // Give it time to drop out.
        }

        private async Task ReadAudioAsync()
        {
            if (!Directory.Exists(ConfigService.BaseDirectory))
            {
                Directory.CreateDirectory(ConfigService.BaseDirectory);
            }

            var newFilename = _fileService.GenerateFileNameWithoutExtension();

            using (var fileStream = new FileStream(_fileService.GetFullPathToNewRecording(newFilename), FileMode.Create, FileAccess.Write))
            {
                while (true)
                {
                    if (_endRecording)
                    {
                        _endRecording = false;
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
            }
            _audioRecord.Stop();
            _audioRecord.Release();
            _fileService.SaveNewMetadataFile(newFilename, ConfigService.AudioFrequency, ConfigService.AudioBitrate);
        }
    }
}