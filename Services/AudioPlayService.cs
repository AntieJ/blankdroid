using System.Threading.Tasks;
using Android.Media;

namespace BlankDroid.Services
{
    public class AudioPlayService
    {
        byte[] _playBuffer = null;
        AudioTrack _audioTrack = null;
        string _fullPath;

        public AudioPlayService(string fullpath)
        {
            _fullPath = fullpath;
        }

        public async Task Start()
        {
            var _fileService = new FileService();
            _playBuffer = _fileService.GetByteArrayFromFile(_fullPath);
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
            _audioTrack = new AudioTrack(
                Android.Media.Stream.Music,
                ConfigService.AudioFrequency,
                ChannelConfiguration.Mono,
                ConfigService.AudioBitrate,
                _playBuffer.Length,
                // Mode. Stream or static.
                AudioTrackMode.Stream);

            _audioTrack.Play();

            await _audioTrack.WriteAsync(_playBuffer, 0, _playBuffer.Length);
        }
    }
}