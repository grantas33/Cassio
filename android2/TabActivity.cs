using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace android2
{
    [Activity(Label = "TabActivity")]
    public class TabActivity : FragmentActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Tabpage);
            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);

            FragmentAdapter adapter = new FragmentAdapter(SupportFragmentManager);
            viewPager.Adapter = adapter;
        }
    }
}

       
    