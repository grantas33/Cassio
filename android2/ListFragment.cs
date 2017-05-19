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
        private string foodName = "";
        private int counter = 1;
        ListView mListView;
        public ListFragments() { }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
           
            var view = inflater.Inflate(Resource.Layout.Listmyfoods, container, false);
            mListView = view.FindViewById<ListView>(Resource.Id.listviewmyfoods);
            var localCalorie = Application.Context.GetSharedPreferences("Calorie", FileCreationMode.Private);
            var CalorieEdit = localCalorie.Edit();
            var localFoods = Application.Context.GetSharedPreferences("Foods", FileCreationMode.Private);
            var FoodEdit = localFoods.Edit();
            var localSavedFoods = Application.Context.GetSharedPreferences("SavedFoods", FileCreationMode.Private);
            var SavedFoodsEdit = localSavedFoods.Edit();
            List<string> output = localSavedFoods.GetStringSet("savedfood", new string[1] { "Empty!" }).ToList();
            output.Sort();
            
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleListItem1, output);
            mListView.Adapter = adapter;

            mListView.ItemClick += (object sender, ItemClickEventArgs e) =>
            {
                if (mListView.GetItemAtPosition(e.Position).ToString() == "Empty!") return;
                MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + GetCalories(mListView.GetItemAtPosition(e.Position).ToString())).ToString();
                MainActivity.foodstring.Append(mListView.GetItemAtPosition(e.Position).ToString() + ';');
                foodName = GetFoodName(mListView.GetItemAtPosition(e.Position).ToString());

                if (toastMsg == string.Format("Added {0} {1}", counter, foodName))
                {
                    counter++;
                }
                else
                {
                    counter = 1;
                }

                toastMsg = string.Format("Added {0} {1}", counter, foodName);
                if (mToast != null) mToast.Cancel();
                mToast = Toast.MakeText(view.Context, toastMsg, ToastLength.Short);
                mToast.Show();

                CalorieEdit.PutString("cal", MainActivity.caloriekeeper);
                CalorieEdit.Apply();


                FoodEdit.PutString("food", MainActivity.foodstring.ToString());
                FoodEdit.Apply();
            };

            mListView.ItemLongClick += (object sender, ItemLongClickEventArgs e) =>
            {
                if (mListView.GetItemAtPosition(e.Position).ToString() == "Empty!") return;
                AlertDialog.Builder alert = new AlertDialog.Builder(view.Context);
                string removedfood = GetFoodName(mListView.GetItemAtPosition(e.Position).ToString());
                alert.SetTitle("Confirmation alert");
                alert.SetMessage(string.Format("Remove {0} from the list?", removedfood));
                alert.SetPositiveButton("Yes", (senderAlert, args) =>
                {
                    
                    output.RemoveAt(e.Position);
                    if (output.Count > 0)
                    {
                        SavedFoodsEdit.PutStringSet("savedfood", output);
                        
                    }
                    else
                    {
                        SavedFoodsEdit.Remove("savedfood");
                    }
                    SavedFoodsEdit.Apply();
                    output = localSavedFoods.GetStringSet("savedfood", new string[1] { "Empty!" }).ToList();
                    adapter = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleListItem1, output);
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

            return view;
        }

        private string GetFoodName(string fullstring)
        {
            string numbers = "0123456789";
            for (int i = fullstring.Length-6; i > 0; i--)
            {
                if (numbers.IndexOf(fullstring[i]) == -1) return fullstring.Substring(0, i - 1);
            }
            return fullstring;
        }

        private int GetCalories(string fullstring)
        {
            string numbers = "0123456789";
            StringBuilder sb = new StringBuilder();
            for (int i = fullstring.Length - 6; i > 0; i--)
            {
                if (numbers.IndexOf(fullstring[i]) != -1) sb.Insert(0, fullstring[i]);
                else return int.Parse(sb.ToString());
            }
            return 0;
        }

    }
}