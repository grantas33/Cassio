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
        ExpandableListView mExpanded;
        SearchView mSearchView;
        public ListFragments() { }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            var view = inflater.Inflate(Resource.Layout.Listmyfoods, container, false);
            mExpanded = view.FindViewById<ExpandableListView>(Resource.Id.expandableviewmyfoods);
            mSearchView = view.FindViewById<SearchView>(Resource.Id.searchviewmyfoods);
            TextView mEmptyView = view.FindViewById<TextView>(Resource.Id.emptymyfoodsview);
            MainActivity.saveddb.foodlist = MainActivity.saveddb.foodlist.OrderBy(foo => foo.Name).ToList();
            var templist = MainActivity.saveddb.foodlist;
            var adapter = new ExpandableListAdapter(this.Activity, templist, true);
            mExpanded.SetAdapter(adapter);
            mExpanded.EmptyView = mEmptyView;
            mExpanded.SetGroupIndicator(null);

            mExpanded.GroupClick += (object sender, ExpandableListView.GroupClickEventArgs e) =>
             {
                ImageView plussign = e.ClickedView.FindViewById<ImageView>(Resource.Id.greenplus);                               //animacija
                Android.Views.Animations.AlphaAnimation plusClick = new Android.Views.Animations.AlphaAnimation(1F, 0.8F);
                plusClick.Duration = 150;
                plussign.StartAnimation(plusClick);

                Food food = new Food(templist[e.GroupPosition]);           
                MainActivity.foodsdb.AddFood(food);
                MainActivity.NutritionEdit.PutString("cal", (int.Parse(MainActivity.localNutritionData.GetString("cal", "0")) + food.Calories).ToString());
                MainActivity.NutritionEdit.Apply();

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

            };

            mExpanded.ItemLongClick += (object sender, ItemLongClickEventArgs e) =>
            {
                
                AlertDialog.Builder alert = new AlertDialog.Builder(view.Context);
                Food removedfood = templist[e.Position];
                alert.SetTitle("Confirmation alert");
                alert.SetMessage(string.Format("Remove {0} from the list?", removedfood));
                alert.SetPositiveButton("Yes", (senderAlert, args) =>
                {

                    MainActivity.saveddb.DeleteFood(removedfood);
                    MainActivity.saveddb.UpdateDatabase();

                    templist.Remove(removedfood);
                    adapter.UpdateAdapter(templist);
                    mExpanded.SetAdapter(adapter);
                
                    if (mToast != null) mToast.Cancel();
                    mToast = Toast.MakeText(view.Context, string.Format("Removed {0}", removedfood.Name ), ToastLength.Short);
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
                mExpanded.SetAdapter(adapter);
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