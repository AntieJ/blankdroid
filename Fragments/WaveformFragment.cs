using Android.OS;
using Android.Views;
using Android.Widget;
using BlankDroid.Services;
using System.Threading.Tasks;

namespace BlankDroid.Fragments
{
    public class WaveformFragment : Android.Support.V4.App.Fragment
    {
        Button _startPlayingButton;
        Button _stopPlayingButton;
        AudioPlayService _audioPlayService;
        Button _updateWaveformButton;
        View waveformView;
        private object view;

        public WaveformFragment() {
            _audioPlayService = new AudioPlayService();
        }

        public static WaveformFragment newInstance()
        {
            WaveformFragment fragment = new WaveformFragment();
            return fragment;
        }

        public override View OnCreateView(
            LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.WaveformFragment, container, false);
            _updateWaveformButton = view.FindViewById<Button>(Resource.Id.updateWaveform);
            _startPlayingButton = view.FindViewById<Button>(Resource.Id.startPlaying);
            _stopPlayingButton = view.FindViewById<Button>(Resource.Id.stopPlaying);
            waveformView = view.FindViewById(Resource.Id.waveformView);
            SetupButtons();
            return view;
        }

        private void SetupButtons()
        {
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