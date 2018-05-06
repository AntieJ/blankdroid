using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using BlankDroid.Services;

namespace BlankDroid.Fragments
{
    public class RecordingInfoFragment : Fragment
    {
        private string _fullAudioPath;
        private string _fileName;
        private string _directory;
        private MetadataService _metadataService;
        private AudioFileService _audioFileService;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _fullAudioPath = AnalysisContext.FullAudioPath;
            _metadataService = new MetadataService();
            _audioFileService = new AudioFileService();

            View view = inflater.Inflate(Resource.Layout.RecordingInfoFragment, container, false);
            view.FindViewById<TextView>(Resource.Id.content).Text = GetContent();
           
            return view;
        }

        private string GetContent()
        {
            var metadata = _metadataService.GetRecordingMetadata($"{ConfigService.BaseDirectory}",$"{AnalysisContext.FileName}");

            return $"Size: {_audioFileService.GetFileSizeInKB(ConfigService.BaseDirectory, AnalysisContext.FileName)}kB "+
                "\n" +
                $"Length: {_audioFileService.GetAudioFileLengthInSeconds(ConfigService.BaseDirectory, AnalysisContext.FileName)}S" +
                "\n" +
                $"Frequency: {metadata.AudioFrequency}" +
                "\n" +
                $"Bitrate: {metadata.AudioBitrate.ToString()}";
        }
    }
}