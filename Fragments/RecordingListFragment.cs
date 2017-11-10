using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.IO;
using BlankDroid.Services;
using System.Collections.Generic;

namespace BlankDroid.Fragments
{
    public class RecordingListFragment : Android.Support.V4.App.ListFragment
    {

        public static RecordingListFragment newInstance()
        {
            return new RecordingListFragment();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            UpdateList();
        }

        public override void OnListItemClick(ListView l, View v, int index, long id)
        {
            var intent = new Intent(Context, typeof(AnalyseActivity));
            intent.PutExtra("ListItemClicked", $"{ConfigService.BaseDirectory}{l.GetItemAtPosition(index)}");
            StartActivity(intent);
        }

        public void UpdateList()
        {
            var values = new List<string>();
            foreach(var value in Directory.GetFiles(ConfigService.BaseDirectory))
            {
                values.Add(value.Replace(ConfigService.BaseDirectory, ""));
            }
            this.ListAdapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleExpandableListItem1, values);
        }
    }
}