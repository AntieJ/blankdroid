using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Widget;
using System.Threading.Tasks;
using BlankDroid.Services;
using Android.Content.PM;

namespace BlankDroid
{
    [Activity(Label = "BlankDroid", ScreenOrientation = ScreenOrientation.Portrait)]
    public class NotesActivity: Activity
    {
        FileService _fileService;
        WaveformService _waveformService;

        public NotesActivity()
        {
            _fileService = new FileService();
            _waveformService = new WaveformService();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.NotesActivity);
            FindViewById<TextView>(Resource.Id.title).Text = "How did it go?";

            var _resultsButton = FindViewById<Button>(Resource.Id.results);

            var values = _fileService.GetFilesFromDirectory(ConfigService.BaseDirectory, ConfigService.AudioFileExtension);

            Task.Run(async () =>
            {
                await ProcessRecordings(ConfigService.BaseDirectory, values);
            });

            _resultsButton.Click += delegate
            {
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




        private async Task ProcessRecordings(string baseDirectory, List<string> fileNames)
        {
            foreach (var file in fileNames)
            {
                await _waveformService.ProcessAndSaveDisplayLines(baseDirectory, file);
            }
        }
    }
}