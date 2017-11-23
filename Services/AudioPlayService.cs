using System.Threading.Tasks;
using Android.Media;
using BlankDroid.Models;

namespace BlankDroid.Services
{
    public class AudioPlayService
    {
        byte[] _playBuffer = null;
        AudioTrack _audioTrack = null;
        FileService _fileService;
        string _fullPathToAudio;
        private RecordingMetadata _metadata;

        public AudioPlayService(string baseDirectory, string fileName)
        {
            _fileService = new FileService();
            _metadata = _fileService.GetRecordingMetadata(baseDirectory, fileName);
            _fullPathToAudio = _fileService.GetFullPathToRecording(baseDirectory, fileName);
        }

        public async Task Start()
        {
            var _fileService = new FileService();
            _playBuffer = _fileService.GetByteArrayFromFile(_fullPathToAudio);
            await PlayAudioTrackAsync();
        }

        public void Stop()
        {
            if (_audioTrack != null)
            {
                _audioTrack.Stop();
                _audioTrack.Release();
                _audioTrack = null;
            }
        }

        private async Task PlayAudioTrackAsync()
        {
            var bitrate = _metadata.AudioBitrate!= Android.Media.Encoding.Invalid ? 
                _metadata.AudioBitrate: ConfigService.AudioBitrate;

            _audioTrack = new AudioTrack(
                Stream.Music,
                ConfigService.AudioFrequency,
                ChannelConfiguration.Mono,
                bitrate,
                _playBuffer.Length,
                // Mode. Stream or static.
                AudioTrackMode.Stream);

            _audioTrack.Play();

            await _audioTrack.WriteAsync(_playBuffer, 0, _playBuffer.Length);
        }
    }
}