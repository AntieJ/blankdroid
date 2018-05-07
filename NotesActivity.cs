using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using BlankDroid.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlankDroid
{
    [Activity(Label = "BlankDroid", ScreenOrientation = ScreenOrientation.Portrait)]
    public class NotesActivity: Activity
    {
        FileService _fileService;
        WaveformService _waveformService;
        private MetadataService _metadataService;

        public NotesActivity()
        {
            _fileService = new FileService();
            _waveformService = new WaveformService();
            _metadataService = new MetadataService();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            HideTitleBar();

            SetContentView(Resource.Layout.NotesActivity);
            FindViewById<TextView>(Resource.Id.title).Text = "Good Morning";
            FindViewById<TextView>(Resource.Id.title).Gravity = GravityFlags.CenterHorizontal;

            var _resultsButton = FindViewById<ImageButton>(Resource.Id.results);

            var values = _fileService.GetFilesFromDirectory(ConfigService.BaseDirectory, ConfigService.AudioFileExtension);

            Task.Run(async () =>
            {
                await ProcessRecordings(ConfigService.BaseDirectory, values);
            });

            _resultsButton.Click += delegate
            {
                SaveNote();
                StartActivity(typeof(AnalyseActivity));
            };

        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        public override void OnBackPressed()
        {
            StartActivity(typeof(MainActivity));

        }

        private void HideTitleBar()
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
        }

        private async Task ProcessRecordings(string baseDirectory, List<string> fileNames)
        {
            foreach (var file in fileNames)
            {
                await _waveformService.ProcessAndSaveDisplayLines(baseDirectory, file);
            }
        }

        private void SaveNote()
        {
            var note = FindViewById<EditText>(Resource.Id.noteText).Text;
            _metadataService.SaveNoteToMetadata(ConfigService.BaseDirectory, RecordingContext.filename, note);
        }
    }
}