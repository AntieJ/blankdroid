using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using BlankDroid.Services;
using System.Threading.Tasks;

namespace BlankDroid
{
    [Activity(Label = "BlankDroid")]
    public class AnalyseActivity : Activity
    {
        Button _startPlayingButton;
        Button _stopPlayingButton;
        AudioPlayService _audioPlayService;
        View waveformView;
        Button _updateWaveformButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            HideTitleBar();
            _audioPlayService = new AudioPlayService();

            SetContentView(Resource.Layout.WaveformFragment);
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

        private void HideTitleBar()
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
        }

        private void SetupButtons()
        {
            _updateWaveformButton = FindViewById<Button>(Resource.Id.updateWaveform);
            _startPlayingButton = FindViewById<Button>(Resource.Id.startPlaying);
            _stopPlayingButton = FindViewById<Button>(Resource.Id.stopPlaying);
            waveformView = FindViewById(Resource.Id.waveformView);

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
                await Task.Run(() =>
                {
                    waveformView.Invalidate();
                    waveformView.RefreshDrawableState();
                });
            };
        }
    }
}

