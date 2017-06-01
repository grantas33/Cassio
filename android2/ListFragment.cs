using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using static Android.Widget.AdapterView;

namespace android2
{
    public class ListFragments : Android.Support.V4.App.Fragment
    {
        private Toast mToast = null;
        private string toastMsg = "";
        private int counter = 1;
        ListView mListView;
        SearchView mSearchView;
        public ListFragments() { }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
           
            var view = inflater.Inflate(Resource.Layout.Listmyfoods, container, false);
            mListView = view.FindViewById<ListView>(Resource.Id.listviewmyfoods);
            mSearchView = view.FindViewById<SearchView>(Resource.Id.searchviewmyfoods);
            var localCalorie = Application.Context.GetSharedPreferences("Calorie", FileCreationMode.Private);
            var CalorieEdit = localCalorie.Edit();
            MainActivity.saveddb.foodlist = MainActivity.saveddb.foodlist.OrderBy(foo => foo.Name).ToList();
            ArrayAdapter<Food> adapter = new ArrayAdapter<Food>(Context, Android.Resource.Layout.SimpleListItem1, MainActivity.saveddb.foodlist);
            mListView.Adapter = adapter;

            mListView.ItemClick += (object sender, ItemClickEventArgs e) =>
            {
                if (mListView.GetItemAtPosition(e.Position).ToString() == "Empty!") return;                              
                Food food = new Food(mListView.GetItemAtPosition(e.Position).ToString());
                MainActivity.foodsdb.AddFood(food);
                MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + food.Calories).ToString();


                if (toastMsg == string.Format("Added {0} {1}", counter, MainActivity.foodsdb.GetLast().Name))
                {
                    counter++;
                }
                else
                {
                    counter = 1;
                }

                
                toastMsg = string.Format("Added {0} {1}", counter, MainActivity.foodsdb.GetLast().Name);
                if (mToast != null) mToast.Cancel();
                mToast = Toast.MakeText(view.Context, toastMsg, ToastLength.Short);
                mToast.Show();

                CalorieEdit.PutString("cal", MainActivity.caloriekeeper);
                CalorieEdit.Apply();

            };

            mListView.ItemLongClick += (object sender, ItemLongClickEventArgs e) =>
            {
                if (mListView.GetItemAtPosition(e.Position).ToString() == "Empty!") return;
                AlertDialog.Builder alert = new AlertDialog.Builder(view.Context);
                string removedfood = MainActivity.saveddb.foodlist[e.Position].Name;
                alert.SetTitle("Confirmation alert");
                alert.SetMessage(string.Format("Remove {0} from the list?", removedfood));
                alert.SetPositiveButton("Yes", (senderAlert, args) =>
                {

                    MainActivity.saveddb.DeleteFood(e.Position);
 
                    adapter = new ArrayAdapter<Food>(Context, Android.Resource.Layout.SimpleListItem1, MainActivity.saveddb.foodlist);
                    mListView.Adapter = adapter;
                
                    if (mToast != null) mToast.Cancel();
                    mToast = Toast.MakeText(view.Context, string.Format("Removed {0}", removedfood ), ToastLength.Short);
                    mToast.Show();
                });

                alert.SetNegativeButton("No", (senderAlert, args) => 
                {

                });

                Dialog dialog = alert.Create();
                dialog.Show();
            };

            mSearchView.QueryTextChange += (object sender, SearchView.QueryTextChangeEventArgs e) =>
            {
                adapter.Filter.InvokeFilter(e.NewText);
            };

            return view;
        }

        public override void OnPause()

        {
            base.OnPause();
            MainActivity.foodsdb.UpdateDatabase();
            MainActivity.saveddb.UpdateDatabase();
        }


    }
}