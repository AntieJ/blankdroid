using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using BlankDroid.Services;
using System;

namespace BlankDroid.Fragments
{
    public class RecordFragment : Android.Support.V4.App.Fragment
    {
        ImageButton _recordingButton;
        //AudioRecordService _audioRecordService;
        TextView _recordingStatus;
        bool _recording;
        FileService _fileService;
        //string _fileName;

        public RecordFragment()
        {
            //_audioRecordService = AudioRecordService;
            _fileService = new FileService();
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

                //start the RECORDING... activity
                

                if (_recording)
                {
                    var intent = new Intent(Context, typeof(RecordingActivity)); ;
                    StartActivity(intent);
                    //OverridePendingTransition(Resource.Anim.left_in, Resource.Anim.left_out);
                
                    RecordingContext.filename = _fileService.GenerateFileNameWithoutExtension();
                    RecordingContext.startedAt = DateTime.Now;
                    _recordingButton.SetBackgroundColor(new Android.Graphics.Color(125, 0, 0));
                    _recordingStatus.Text = "Recording...";
                    await AudioRecordService.Start(RecordingContext.filename);

                    
                }
                else
                {
                    //this is done by the recording activity now

                    //_recordingButton.SetBackgroundColor(new Android.Graphics.Color(0, 125, 0));
                    //_recordingStatus.Text = "Click to Record";


                    //AudioRecordService.Stop();
                    //_fileService.SaveNewMetadataFile(RecordingContext.filename, 
                    //    RecordingContext.startedAt,
                    //    ConfigService.AudioFrequency, 
                    //    ConfigService.AudioBitrate, 
                    //    FactorService.GetContext(), 
                    //    null);
                    //UpdateRecordingsList();
                }
            };
        }

        private void UpdateRecordingsList()
        {
            AnalysisContext.adaptor.UpdateList();
        }
    }
}