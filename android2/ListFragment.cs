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
            TextView mEmptyView = view.FindViewById<TextView>(Resource.Id.emptymyfoodsview);
            var localCalorie = Application.Context.GetSharedPreferences("Calorie", FileCreationMode.Private);
            var CalorieEdit = localCalorie.Edit();
            MainActivity.saveddb.foodlist = MainActivity.saveddb.foodlist.OrderBy(foo => foo.Name).ToList();
            var templist = MainActivity.saveddb.foodlist;

           foreach (var i in MainActivity.saveddb.foodlist)
            {
                System.Diagnostics.Debug.WriteLine(i);
                System.Diagnostics.Debug.WriteLine("---");
            }

            var adapter = new FoodRowListAdapter(this.Activity, templist);
            mListView.Adapter = adapter;
            mListView.EmptyView = mEmptyView;

            mListView.ItemClick += (object sender, ItemClickEventArgs e) =>
            {

                Food food = new Food(templist[e.Position]);

                
                MainActivity.foodsdb.AddFood(food);
                MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + food.Calories).ToString();
                foreach (var i in MainActivity.saveddb.foodlist)
                {
                    System.Diagnostics.Debug.WriteLine(i);
                    System.Diagnostics.Debug.WriteLine("---");
                }

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
                AlertDialog.Builder alert = new AlertDialog.Builder(view.Context);
                Food removedfood = templist[e.Position];
                alert.SetTitle("Confirmation alert");
                alert.SetMessage(string.Format("Remove {0} from the list?", removedfood.Name));
                alert.SetPositiveButton("Yes", (senderAlert, args) =>
                {

                    MainActivity.saveddb.DeleteFood(removedfood);
                    MainActivity.saveddb.UpdateDatabase();

                    templist.Remove(removedfood);
                    adapter.UpdateAdapter(templist);
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
                templist = MainActivity.saveddb.foodlist.Where(foo => foo.Name.IndexOf(e.NewText, StringComparison.OrdinalIgnoreCase) >= 0).Select(foo => foo).ToList();
                adapter.UpdateAdapter(templist);
                mListView.Adapter = adapter;
                if (MainActivity.saveddb.foodlist.Count > 0) mEmptyView.Text = "No results!";
                else mEmptyView.Text = "You haven't added any foods!";
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