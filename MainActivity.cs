using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using BlankDroid.Services;
using Android.Views;

namespace BlankDroid
{
    [Activity(Label = "BlankDroid", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button _startRecordingButton;
        Button _stopRecordingButton;
        Button _startPlayingButton;
        Button _stopPlayingButton;
        Button _updateWaveformButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            HideTitleBar();
            SetContentView(Resource.Layout.Main);

            SetupButtons();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        private void SetupButtons()
        {
            _startRecordingButton = FindViewById<Button>(Resource.Id.start);
            _stopRecordingButton = FindViewById<Button>(Resource.Id.stop);
            _startPlayingButton = FindViewById<Button>(Resource.Id.startPlaying);
            _stopPlayingButton = FindViewById<Button>(Resource.Id.stopPlaying);
            _updateWaveformButton = FindViewById<Button>(Resource.Id.updateWaveform);

            var _audioRecordService = new AudioRecordService();
            var _audioPlayService = new AudioPlayService();

            _startRecordingButton.Click += async delegate
            {
                _stopRecordingButton.Enabled = !_stopRecordingButton.Enabled;
                _startRecordingButton.Enabled = !_startRecordingButton.Enabled;

                await _audioRecordService.Start();
            };

            _stopRecordingButton.Click += delegate
            {
                _stopRecordingButton.Enabled = !_stopRecordingButton.Enabled;
                _startRecordingButton.Enabled = !_startRecordingButton.Enabled;

                _audioRecordService.Stop();
            };

            _startPlayingButton.Click += async delegate
            {
                _stopPlayingButton.Enabled = !_stopPlayingButton.Enabled;
                _startPlayingButton.Enabled = !_startPlayingButton.Enabled;

                await _audioPlayService.Start();
            };

            _stopPlayingButton.Click += delegate
            {
                _stopPlayingButton.Enabled = !_stopPlayingButton.Enabled;
                _startPlayingButton.Enabled = !_startPlayingButton.Enabled;
                _audioPlayService.Stop();
            };

            _updateWaveformButton.Click += async delegate
            {
                Task.Run(() =>
                {
                    

                    var waveformView = FindViewById(Resource.Id.waveformView);

                    waveformView.Invalidate();
                    waveformView.RefreshDrawableState();
                });
            };            
        }

        private void HideTitleBar()
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
        }
    }
}

