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
using Android.Support.V7.App;

namespace android2
{
    [Activity(Label = "GridActivity")]
    public class GridActivity : AppCompatActivity
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

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = GetString(Resource.String.food_log);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_arrow_back_white_24dp);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            mExpandable.EmptyView = emptytext;
            mExpandable.SetGroupIndicator(null);       

            var adapter = new FoodListAdapter(this, MainActivity.foodsdb.datalist, false);
            mExpandable.SetAdapter(adapter);


            undolastbutt.Click += (sender, e) =>
            {
                if (MainActivity.foodsdb.datalist.Count() == 0) return;
                var state = mExpandable.OnSaveInstanceState();
                MainActivity.NutritionEdit.PutString("cal", (int.Parse(MainActivity.localNutritionData.GetString("cal", "0")) - MainActivity.foodsdb.GetLast().Calories).ToString());
                MainActivity.NutritionEdit.Apply();

                MainActivity.foodsdb.DeleteData(MainActivity.foodsdb.datalist.Count() - 1);

                adapter.UpdateAdapter(MainActivity.foodsdb.datalist);
                mExpandable.SetAdapter(adapter);
                mExpandable.OnRestoreInstanceState(state);
            };

        }


        protected override void OnPause()
        {
            base.OnPause();
            MainActivity.foodsdb.UpdateDatabase();
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