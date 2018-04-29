using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using BlankDroid.Services;
using static Android.Widget.TextView;

namespace BlankDroid.Fragments
{
    public class FactorsInputFragment : Fragment
    {
        private string[] Factors = { "Alcohol", "Caffeine", "Smoking" };

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.FactorsInputFragment, container, false);
            AddFactorCheckboxes(view);
            return view;
        }        

        private void AddFactorCheckboxes(View view)
        {
            foreach(var factor in Factors)
            {
                CheckBox cb = new CheckBox(view.Context);
                cb.SetText(factor, BufferType.Normal);
                LinearLayout ll = (LinearLayout)view.FindViewById(Resource.Id.factorsLinearLayout);
                ll.AddView(cb);
                cb.Click += delegate
                {
                    FactorService.UpdateContext(factor, cb.Checked);
                };
            }            
        }
    }
}