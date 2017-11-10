using Android.App;
using Android.OS;
using Android.Views;
using Android.Support.V4.View;
using Android.Support.V4.App;

namespace BlankDroid
{
    [Activity(Label = "BlankDroid", MainLauncher = true)]
    public class MainActivity : FragmentActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            HideTitleBar();
            SetContentView(Resource.Layout.Main);
            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            MainAdaptor adapter = new MainAdaptor(SupportFragmentManager);
            viewPager.Adapter = adapter;
            AnalysisContext.adaptor = adapter;
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        private void HideTitleBar()
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
        }
    }
}

