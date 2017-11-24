using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using BlankDroid.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlankDroid.Fragments
{
    public class RecordingListFragment : Android.Support.V4.App.ListFragment
    {
        FileService _fileService;
        WaveformService _waveformService;

        public RecordingListFragment()
        {
            _fileService = new FileService();
            _waveformService = new WaveformService();
        }

        public static RecordingListFragment newInstance()
        {
            return new RecordingListFragment();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            UpdateList();
        }

        public override void OnListItemClick(ListView l, View v, int index, long id)
        {
            var intent = new Intent(Context, typeof(AnalyseActivity));
            intent.PutExtra("FileNameClicked", $"{l.GetItemAtPosition(index)}");
            StartActivity(intent);
        }

        public void UpdateList()
        {
            var values = _fileService.GetFilesFromDirectory(ConfigService.BaseDirectory, ConfigService.AudioFileExtension);
            this.ListAdapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleExpandableListItem1, values);

            Task.Run(async () =>
            {
                await ProcessRecordings(ConfigService.BaseDirectory, values);
            });
        }

        private async Task ProcessRecordings(string baseDirectory, List<string> fileNames)
        {
            foreach( var file in fileNames)
            {
                await _waveformService.ProcessAndSaveDisplayLines(baseDirectory, file);
            }
        }
    }
}