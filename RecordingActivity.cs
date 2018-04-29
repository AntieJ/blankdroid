using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BlankDroid.Services;
using static Android.Support.V4.View.ViewPager;
using Android.Content.PM;

namespace BlankDroid
{
    [Activity(Label = "BlankDroid", ScreenOrientation = ScreenOrientation.Portrait)]
    public class RecordingActivity : Activity
    {
        //AudioRecordService _audioRecordService;
        FileService _fileService;

        public RecordingActivity()
        {
            //_audioRecordService = new AudioRecordService();
            _fileService = new FileService();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);
           
            SetContentView(Resource.Layout.RecordingActivity);
            FindViewById<TextView>(Resource.Id.title).Text = "ZZZ.... Sleep well... :)";

           var _stopButton = FindViewById<Button>(Resource.Id.stopRecording);

            _stopButton.Click += delegate
            {
                AudioRecordService.Stop();
                _fileService.SaveNewMetadataFile(RecordingContext.filename, ConfigService.AudioFrequency, ConfigService.AudioBitrate, FactorService.GetContext(), null);
                UpdateRecordingsList();

                StartActivity(typeof(NotesActivity));
            };
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        private void UpdateRecordingsList()
        {
            AnalysisContext.adaptor.UpdateList();
        }
    }
}