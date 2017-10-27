using Android.OS;
using Android.Views;
using Android.Widget;
using BlankDroid.Services;

namespace BlankDroid.Fragments
{
    public class PlayFragment : Android.Support.V4.App.Fragment
    {
        Button _startPlayingButton;
        Button _stopPlayingButton;
        AudioPlayService _audioPlayService;

        public PlayFragment() {
            _audioPlayService = new AudioPlayService();
        }

        public static PlayFragment newInstance()
        {
            PlayFragment fragment = new PlayFragment();
            return fragment;
        }

        public override View OnCreateView(
            LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.PlayFragment, container, false);
            _startPlayingButton = view.FindViewById<Button>(Resource.Id.startPlaying);
            _stopPlayingButton = view.FindViewById<Button>(Resource.Id.stopPlaying);
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

        }
    }
}