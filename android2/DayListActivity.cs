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
using Android.Support.V7.App;

namespace android2
{
    [Activity(Label = "DayListActivity")]
    public class DayListActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.DayListPage);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = GetString(Resource.String.daily_view);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_arrow_back_white_24dp);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            ExpandableListView lv = FindViewById<ExpandableListView>(Resource.Id.listdays);
            TextView empty = FindViewById<TextView>(Resource.Id.emptydayview);
            Button clearfinalbutt = FindViewById<Button>(Resource.Id.clearfinalbutton);
            var adapter = new DayListAdapter(this, MainActivity.daysdb.datalist);
            lv.SetAdapter(adapter);
            lv.SetGroupIndicator(null);
            lv.EmptyView = empty;
            
            clearfinalbutt.Click += (sender, e) =>
            {
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Confirmation alert");
                alert.SetMessage("Do you really want erase all history data?");
                alert.SetPositiveButton("Yes", (senderAlert, args) => {
                    MainActivity.daysdb.ClearAll();
                    MainActivity.daysdb.UpdateDatabase();
                    Toast.MakeText(this, "History cleared!", ToastLength.Short).Show();
                    adapter.UpdateAdapter(MainActivity.daysdb.datalist);
                    lv.SetAdapter(adapter);
                });

                alert.SetNegativeButton("No", (senderAlert, args) => {

                });

                Dialog dialog = alert.Create();
                dialog.Show();
            };
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}