using System.IO;

namespace BlankDroid.Services
{
    public class AudioFileService
    {
        private MetadataService _metadataService;
        private FileService _fileService;

        public AudioFileService()
        {
            _metadataService = new MetadataService();
            _fileService = new FileService();
        }

        public long GetFileSizeInKB(string baseDirectory, string fileName)
        {
            var metadata = _metadataService.GetRecordingMetadata(baseDirectory, fileName);
            if (metadata.FileSizeKb != -1 && metadata.FileSizeKb != 0)
            {
                return metadata.FileSizeKb;
            }
            var fullRecordingPath = GetFullPathToRecording(baseDirectory, fileName);
            metadata.FileSizeKb = _fileService.GetFileSizeInKBDirectlyFromFile(fullRecordingPath);
            _metadataService.UpdateMetadataFile(metadata, baseDirectory, fileName);

            return metadata.FileSizeKb;
        }

        public int GetAudioFileLengthInSeconds(string baseDirectory, string fileName)
        {
            var metadata = _metadataService.GetRecordingMetadata(baseDirectory, fileName);
            if (metadata.FileLengthSeconds != -1 && metadata.FileLengthSeconds != 0)
            {
                return metadata.FileLengthSeconds;
            }
            var fullRecordingPath = GetFullPathToRecording(baseDirectory, fileName);
            metadata.FileLengthSeconds = GetAudioFileLengthInSecondsDirectlyFromFile(fullRecordingPath);
            _metadataService.UpdateMetadataFile(metadata, baseDirectory, fileName);

            return metadata.FileLengthSeconds;
        }

        public bool TryDeleteByFileName(string baseDirectory, string fileName)
        {
            try
            {
                var audioPath = GetFullPathToRecording(baseDirectory, fileName);
                var metadataPath = _metadataService.GetFullPathToMetadata(baseDirectory, fileName);
                var displayLinesPath = _fileService.GetFullPathToDisplayLinesFile(baseDirectory, fileName);
                _fileService.DeleteFile(audioPath);
                _fileService.DeleteFile(metadataPath);
                _fileService.DeleteFile(displayLinesPath);

                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public string GetFullPathToNewRecording(string fileName)
        {
            return GetFullPathToRecording(ConfigService.BaseDirectory, fileName);
        }

        public string GetFullPathToRecording(string baseDirectory, string fileName)
        {
            return $"{baseDirectory}{fileName}{ConfigService.AudioFileExtension}";
        }

        private int GetAudioFileLengthInSecondsDirectlyFromFile(string filepath)
        {
            var byteArray = _fileService.GetByteArrayFromFile(filepath);
            return (byteArray.Length / 2) / ConfigService.AudioFrequency;
        }
    }
}