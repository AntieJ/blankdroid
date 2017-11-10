using Android.OS;
using Android.Views;
using Android.Widget;
using BlankDroid.Services;

namespace BlankDroid.Fragments
{
    public class RecordFragment : Android.Support.V4.App.Fragment
    {
        ImageButton _recordingButton;
        AudioRecordService _audioRecordService;
        TextView _recordingStatus;
        bool _recording;

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

            _recordingButton = view.FindViewById<ImageButton>(Resource.Id.recordButton);
            _recordingButton.SetBackgroundColor(new Android.Graphics.Color(0, 125, 0));
            SetupButtons();


            _recordingStatus = view.FindViewById<TextView>(Resource.Id.recordingStatus);
            _recordingStatus.Text = "Click to Record";

            _recording = false;
            return view;
        }

        private void SetupButtons()
        {
            _recordingButton.Click += async delegate
            {
                _recording = !_recording;
                if (_recording)
                {
                    _recordingButton.SetBackgroundColor(new Android.Graphics.Color(125, 0, 0));
                    _recordingStatus.Text = "Recording...";
                    await _audioRecordService.Start();
                }
                else
                {
                    _recordingButton.SetBackgroundColor(new Android.Graphics.Color(0, 125, 0));
                    _recordingStatus.Text = "Click to Record";


                    _audioRecordService.Stop();
                    AnalysisContext.adaptor.UpdateList();
                }
            };
        }
    }
}