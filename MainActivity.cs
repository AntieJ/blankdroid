using Android.App;
using Android.OS;
using Android.Views;
using Android.Support.V4.View;
using Android.Support.V4.App;
using BlankDroid.Services;
using Android.Content.PM;

namespace BlankDroid
{
    [Activity(Label = "BlankDroid", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : FragmentActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            HideTitleBar();
            ConfigService.PixelWidth = Resources.DisplayMetrics.WidthPixels;
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

