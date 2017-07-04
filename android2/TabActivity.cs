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
using Android.Support.V7.App;

namespace android2
{
    [Activity(Label = "TabActivity")]
    public class TabActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Tabpage);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Choose food";
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_arrow_back_white_24dp);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);


            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);

            FragmentAdapter adapter = new FragmentAdapter(SupportFragmentManager);
            viewPager.Adapter = adapter;
        }
    }
}

       
    