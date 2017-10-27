using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.IO;
using BlankDroid.Services;

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
            var values = Directory.GetFiles(ConfigService.BaseDirectory);
            this.ListAdapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleExpandableListItem1, values);
            
        }

        public override void OnListItemClick(ListView l, View v, int index, long id)
        {
            Toast.MakeText(Context, "Item "+id, ToastLength.Short).Show();

            var intent = new Intent(Context, typeof(AnalyseActivity));
            //intent.PutStringArrayListExtra("phone_numbers", phoneNumbers);
            StartActivity(intent);
        }
    }
}