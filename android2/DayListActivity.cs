﻿using System;
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
            ListView lv = FindViewById<ListView>(Resource.Id.listdays);
            TextView empty = FindViewById<TextView>(Resource.Id.emptydayview);
            Button clearfinalbutt = FindViewById<Button>(Resource.Id.clearfinalbutton);
            var localDays = Application.Context.GetSharedPreferences("Days", FileCreationMode.Private);
            string[] alldays = localDays.GetString("days", "").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            lv.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, alldays);
            lv.EmptyView = empty;
            
            clearfinalbutt.Click += (sender, e) =>
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Confirmation alert");
                alert.SetMessage("Do you really want erase all history data?");
                alert.SetPositiveButton("Yes", (senderAlert, args) => {
                    var daysEdit = localDays.Edit();
                    daysEdit.Remove("days");
                    MainActivity.daystring.Clear();
                    daysEdit.Apply();
                    Toast.MakeText(this, "History cleared!", ToastLength.Short).Show();
                    lv.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, new string[] { });
                });

                alert.SetNegativeButton("No", (senderAlert, args) => {

                });

                Dialog dialog = alert.Create();
                dialog.Show();
            };
        }
    }
}