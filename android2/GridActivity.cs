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
using static Android.Widget.ExpandableListView;

namespace android2
{
    [Activity(Label = "GridActivity")]
    public class GridActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var localCalorie = Application.Context.GetSharedPreferences("Calorie", FileCreationMode.Private);
            var CalorieEdit = localCalorie.Edit();


            SetContentView(Resource.Layout.Gridpage);
            Button undolastbutt = FindViewById<Button>(Resource.Id.undolastbutton);
            View emptytext = FindViewById(Resource.Id.emptyfoodlog);
            ExpandableListView mExpandable = FindViewById<ExpandableListView>(Resource.Id.expandablelistfoodlog);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            mExpandable.EmptyView = emptytext;
            mExpandable.SetGroupIndicator(null);       
            SetActionBar(toolbar);
            ActionBar.Title = "Food log";

            var adapter = new ExpandableListAdapter(this, MainActivity.foodsdb.foodlist, false);
            mExpandable.SetAdapter(adapter);


            undolastbutt.Click += (sender, e) =>
            {
                if (MainActivity.foodsdb.foodlist.Count() == 0) return;
                var state = mExpandable.OnSaveInstanceState();
                MainActivity.NutritionEdit.PutString("cal", (int.Parse(MainActivity.localNutritionData.GetString("cal", "0")) - MainActivity.foodsdb.GetLast().Calories).ToString());
                MainActivity.NutritionEdit.Apply();

                MainActivity.foodsdb.DeleteLast();

                adapter.UpdateAdapter(MainActivity.foodsdb.foodlist);
                mExpandable.SetAdapter(adapter);
                mExpandable.OnRestoreInstanceState(state);
            };

        }


        protected override void OnPause()
        {
            base.OnPause();
            MainActivity.foodsdb.UpdateDatabase();
        }


    }
}