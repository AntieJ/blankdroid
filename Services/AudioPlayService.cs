using System.Threading.Tasks;
using Android.Media;
using BlankDroid.Models;

namespace BlankDroid.Services
{
    public class AudioPlayService
    {
        byte[] _playBuffer = null;
        AudioTrack _audioTrack = null;
        string _fullPathToAudio;
        private RecordingMetadata _metadata;
        private MetadataService _metadataService;
        private AudioFileService _audioFileService;

        public AudioPlayService(string baseDirectory, string fileName)
        {
            _metadataService = new MetadataService();
            _audioFileService = new AudioFileService();
            _metadata = _metadataService.GetRecordingMetadata(baseDirectory, fileName);
            _fullPathToAudio = _audioFileService.GetFullPathToRecording(baseDirectory, fileName);
        }

        public async Task Start()
        {
            //TODO: can the play buffer be built when the user loads the page but before they press start?
            //will this be more performant?
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