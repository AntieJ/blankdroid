using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using BlankDroid.Services;

namespace BlankDroid
{
    [Activity(Label = "BlankDroid")]
    public class AnalyseActivity : Activity
    {
        Button _startPlayingButton;
        Button _stopPlayingButton;
        Button _deleteRecordingButton;

        AudioPlayService _audioPlayService;
        FileService _fileService;

        string path;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            HideTitleBar();
            path = Intent.GetStringExtra("ListItemClicked") ?? "Data not available";
            AnalysisContext.UpdateContext(path);
            _audioPlayService = new AudioPlayService(path);
            _fileService = new FileService();
            ConfigService.DirectoryToAnalyse = path;
            SetContentView(Resource.Layout.AnalyseFragment);
            SetupButtons();
            FindViewById<TextView>(Resource.Id.title).Text = path.Replace(ConfigService.BaseDirectory,"");

        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        private void HideTitleBar()
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
        }

        private void SetupButtons()
        {
            _startPlayingButton = FindViewById<Button>(Resource.Id.startPlaying);
            _stopPlayingButton = FindViewById<Button>(Resource.Id.stopPlaying);
            _deleteRecordingButton = FindViewById<Button>(Resource.Id.deleteRecording);

            _startPlayingButton.Click += async delegate
            {
                _stopPlayingButton.Enabled = !_stopPlayingButton.Enabled;
                _startPlayingButton.Enabled = !_startPlayingButton.Enabled;

                await _audioPlayService.Start();
            };

            _stopPlayingButton.Click += delegate
            {
                StopPlaying();
            };

            _deleteRecordingButton.Click += delegate
            {
                StopPlaying();
                var deleteSucceeded = _fileService.TryDelete(path);
                if (deleteSucceeded)
                {
                    AnalysisContext.adaptor.UpdateList();
                    Toast.MakeText(ApplicationContext, "Deleted!", ToastLength.Short).Show();
                    Finish();
                }
                else
                {
                    Toast.MakeText(ApplicationContext, "Failed to delete", ToastLength.Short).Show();
                }

            };
        }

        private void StopPlaying()
        {
            _stopPlayingButton.Enabled = !_stopPlayingButton.Enabled;
            _startPlayingButton.Enabled = !_startPlayingButton.Enabled;
            _audioPlayService.Stop();
        }

        
    }
}

