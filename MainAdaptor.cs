using Android.Support.V4.App;
using BlankDroid.Fragments;

namespace BlankDroid
{
    public class MainAdaptor : FragmentPagerAdapter
    {
        RecordingListFragment recordingListFragment;

        public MainAdaptor(Android.Support.V4.App.FragmentManager fm)
            : base(fm) { }

        public override int Count
        {
            get { return 2; }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            if (position == 0)
            {
                return (Android.Support.V4.App.Fragment)RecordFragment.newInstance();

            }
            else
            {
                recordingListFragment = RecordingListFragment.newInstance();
                return recordingListFragment;

            }
        }

        public void UpdateList()
        {
            recordingListFragment.UpdateList();
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            if (position == 0)
            {
                return new Java.Lang.String("Record");

            }
            else
            {
                return new Java.Lang.String("Analyse");
            }

        }
    }
}