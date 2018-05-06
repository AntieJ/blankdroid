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
using System.Threading.Tasks;

namespace BlankDroid
{
    [Activity(Label = "BlankDroid", ScreenOrientation = ScreenOrientation.Portrait)]
    public class RecordingActivity : Activity
    {
        WaveformService _waveformService;
        private MetadataService _metadataService;

        public RecordingActivity()
        {
            _waveformService = new WaveformService();
            _metadataService = new MetadataService();
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
                StopRecording();

                Task.Run(async () =>
                {
                    await ProcessRecording(ConfigService.BaseDirectory, RecordingContext.filename);
                });
                
                //UpdateRecordingsList();

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

        public override void OnBackPressed()
        {
            RequestStopConfirmation();
        }

        private void RequestStopConfirmation()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Confirm stop");
            alert.SetMessage("Are you sure you want to stop this recording?");
            alert.SetPositiveButton("Stop", (senderAlert, args) => {
                StopRecording();
                StartActivity(typeof(MainActivity));
            });

            alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                Toast.MakeText(this, "Recording...", ToastLength.Short).Show();
            });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        private void StopRecording()
        {
            AudioRecordService.Stop();
            _metadataService.SaveNewMetadataFile(RecordingContext.filename,
                    RecordingContext.startedAt,
                    ConfigService.AudioFrequency,
                    ConfigService.AudioBitrate,
                    FactorService.GetContext(),
                    null);
        }

        private void UpdateRecordingsList()
        {
            AnalysisContext.adaptor.UpdateList();
        }

        private async Task ProcessRecording(string baseDirectory, string fileName)
        {
            await _waveformService.ProcessAndSaveDisplayLines(baseDirectory, fileName);
        }
    }
}