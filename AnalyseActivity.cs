using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using BlankDroid.Services;
using System.IO;

namespace BlankDroid
{
    [Activity(Label = "BlankDroid")]
    public class AnalyseActivity : Activity
    {
        private ImageButton _deleteRecordingButton;
        private ImageButton _playPauseButton;
        private bool _playing;
        private AudioPlayService _audioPlayService;
        private FileService _fileService;
        private string _fileName;
        private string _baseDirectory;
        private string _fullAudioPath;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            HideTitleBar();

            _fileService = new FileService();
            _fileName = Intent.GetStringExtra("FileNameClicked") ?? "";
            _baseDirectory = ConfigService.BaseDirectory;
            _fullAudioPath = _fileService.GetFullPathToRecording(_baseDirectory, _fileName);
            _audioPlayService = new AudioPlayService(_baseDirectory, _fileName);
            
            AnalysisContext.UpdateContext(_baseDirectory, _fileName);
            SetContentView(Resource.Layout.AnalyseActivity);
            SetupButtons();
            FindViewById<TextView>(Resource.Id.title).Text = _fullAudioPath.Replace(_baseDirectory,"");
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
                RequestDeleteConfirmation();              

            };
        }

        private void RequestDeleteConfirmation()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Confirm delete");
            alert.SetMessage("Are you sure you want to delete this recording?");
            alert.SetPositiveButton("Delete", (senderAlert, args) => {
                DeleteFile();
            });

            alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();                
            });

            Dialog dialog = alert.Create();
            dialog.Show();
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

        private void DeleteFile()
        {
            var deleteSucceeded = _fileService.TryDeleteByFileName(_baseDirectory, _fileName);
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
        }
    }
}

