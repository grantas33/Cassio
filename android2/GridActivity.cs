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
            var mListView = FindViewById<ListView>(Resource.Id.listfoodlog);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "Food log";


            ArrayAdapter<Food> adapter = new ArrayAdapter<Food>(this, Android.Resource.Layout.SimpleListItem1, MainActivity.foodsdb.foodlist);
            mListView.Adapter = adapter;



            undolastbutt.Click += (sender, e) =>
            {
                if (MainActivity.foodsdb.foodlist.Count() == 0) return;
                var state = mListView.OnSaveInstanceState();
                MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) - MainActivity.foodsdb.GetLast().Calories).ToString();
                CalorieEdit.PutString("cal", MainActivity.caloriekeeper);
                CalorieEdit.Apply();

                MainActivity.foodsdb.DeleteLast();

                adapter = new ArrayAdapter<Food>(this, Android.Resource.Layout.SimpleListItem1, MainActivity.foodsdb.foodlist);
                mListView.Adapter = adapter;
                mListView.OnRestoreInstanceState(state);
            };

        }


        protected override void OnPause()
        {
            base.OnPause();
            MainActivity.foodsdb.UpdateDatabase();
        }


    }
}