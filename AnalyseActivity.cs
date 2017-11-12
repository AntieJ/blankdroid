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
        ImageButton _deleteRecordingButton;
        ImageButton _playPauseButton;
        bool _playing;
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
            SetContentView(Resource.Layout.AnalyseActivity);
            SetupButtons();
            FindViewById<TextView>(Resource.Id.title).Text = path.Replace(ConfigService.BaseDirectory,"");
            _playing = false;
            SetPlayButtonIcon();

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
            _deleteRecordingButton = FindViewById<ImageButton>(Resource.Id.deleteRecording);
            _playPauseButton = FindViewById<ImageButton>(Resource.Id.playPauseButton);

            _playPauseButton.Click += async delegate
            {
                if (!_playing)
                {
                    _playing = !_playing;
                    _playPauseButton.SetBackgroundColor(new Android.Graphics.Color(125, 0, 0));
                    _playPauseButton.SetImageResource(Resource.Drawable.pause);
                    await _audioPlayService.Start();
                }
                else
                {
                    _playing = !_playing;
                    SetPlayButtonIcon();
                    StopPlaying();
                }
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
            _audioPlayService.Stop();
        }

        private void SetPlayButtonIcon()
        {
            _playPauseButton.SetBackgroundColor(new Android.Graphics.Color(0, 125, 0));
            _playPauseButton.SetImageResource(Resource.Drawable.play);
        }
        
    }
}

