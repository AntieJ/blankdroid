using Android.OS;
using Android.Views;
using Android.Widget;
using BlankDroid.Services;

namespace BlankDroid.Fragments
{
    public class RecordFragment : Android.Support.V4.App.Fragment
    {
        Button _startRecordingButton;
        Button _stopRecordingButton;
        AudioRecordService _audioRecordService;

        public RecordFragment() {
            _audioRecordService = new AudioRecordService();
        }

        public static RecordFragment newInstance()
        {
            RecordFragment fragment = new RecordFragment();
            return fragment;
        }

        public override View OnCreateView(
            LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.RecordFragment, container, false);
            _startRecordingButton = view.FindViewById<Button>(Resource.Id.start);
            _stopRecordingButton = view.FindViewById<Button>(Resource.Id.stop);
            SetupButtons();

            return view;
        }

        private void SetupButtons()
        {
            
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
                AnalysisContext.adaptor.UpdateList();
            };

            
        }
    }
}