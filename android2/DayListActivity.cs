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

namespace android2
{
    [Activity(Label = "DayListActivity")]
    public class DayListActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.DayListPage);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "Daily view";
            ExpandableListView lv = FindViewById<ExpandableListView>(Resource.Id.listdays);
            TextView empty = FindViewById<TextView>(Resource.Id.emptydayview);
            Button clearfinalbutt = FindViewById<Button>(Resource.Id.clearfinalbutton);
            var adapter = new DayListAdapter(this, MainActivity.daysdb.datalist);
            lv.SetAdapter(adapter);
            lv.SetGroupIndicator(null);
            lv.EmptyView = empty;
            
            clearfinalbutt.Click += (sender, e) =>
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
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
    }
}