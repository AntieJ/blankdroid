using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using BlankDroid.Services;

namespace BlankDroid.Fragments
{
    public class RecordingInfoFragment : Fragment
    {
        private string _path;
        FileService _fileService;

        public override View OnCreateView(
            LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _path = ConfigService.DirectoryToAnalyse;
            _fileService = new FileService();
            View view = inflater.Inflate(Resource.Layout.RecordingInfoFragment, container, false);
            view.FindViewById<TextView>(Resource.Id.content).Text = GetContent();
           
            return view;
        }

        private string GetContent()
        {
            return $"Size: {_fileService.GetFileSizeInKB(_path)}kB "+
                "\n" +
                $"Length: {_fileService.GetFileLengthInSeconds(_path)}S" +
                "\n" +
                "Date";
        }
    }
}