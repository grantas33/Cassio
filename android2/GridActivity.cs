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

            var localFoods = Application.Context.GetSharedPreferences("Foods", FileCreationMode.Private);
            var localEdit = localFoods.Edit();
            var localCalorie = Application.Context.GetSharedPreferences("Calorie", FileCreationMode.Private);
            var CalorieEdit = localCalorie.Edit();


            SetContentView(Resource.Layout.Gridpage);
            Button undolastbutt = FindViewById<Button>(Resource.Id.undolastbutton);
            var mListView = FindViewById<ListView>(Resource.Id.listfoodlog);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);          
            SetActionBar(toolbar);
            ActionBar.Title = "Food log";

            string[] allfoods = localFoods.GetString("food", "Empty!").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);   
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, RemoveDuplicates(allfoods));
            mListView.Adapter = adapter;
            

            undolastbutt.Click += (sender, e) =>
            {
                var state = mListView.OnSaveInstanceState();
                MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) - GetCalories(MainActivity.foodstring.ToString())).ToString();
                CalorieEdit.PutString("cal", MainActivity.caloriekeeper);
                CalorieEdit.Apply();

                localEdit.PutString("food", RemoveLastFood(localFoods.GetString("food", "Empty!")));
                localEdit.Apply();
                MainActivity.foodstring.Clear();
                MainActivity.foodstring.Append(localFoods.GetString("food", null));
                allfoods = localFoods.GetString("food", "Empty!").Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                
                
                adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, RemoveDuplicates(allfoods));
                mListView.Adapter = adapter;
                mListView.OnRestoreInstanceState(state);
            };

        }


        private string[] RemoveDuplicates(string[] lines)
        {
            int counter = 2;
            string original = "";
            int stopper = -1;
            List<string> linelist = lines.ToList();
            for (int i = 0; i < linelist.Count-1; i++)
            {
                if (GetFoodName(linelist[i]) == GetFoodName(linelist[i + 1]))
                {

                    if (stopper != i) original = linelist[i];                    
                    stopper = i;
                    linelist[i] = original + " x" + counter;
                    linelist.RemoveAt(i + 1);
                    i--;
                    counter++;
                }
                else counter = 2;
            }

            return linelist.ToArray();
        
        }

        private string RemoveLastFood(string original)
        {
            for (int i = original.Length - 2; i > 0 ; i--)
            {
                if (original[i] == ';') return original.Substring(0, i+1);
            }
            return null;
        }

        private int GetCalories(string fullstring)
        {
            string numbers = "0123456789";
            StringBuilder sb = new StringBuilder();
            for (int i = fullstring.Length - 7; i > 0; i--)
            {
                if (numbers.IndexOf(fullstring[i]) != -1) sb.Insert(0, fullstring[i]);
                else return int.Parse(sb.ToString());
            }
            return 0;
        }

        private string GetFoodName(string fullstring)
        {
            //string numbers = "0123456789 cal.x";
            for (int i = fullstring.Length -1; i > 0; i--)
            {
                if (fullstring[i] == ',') return fullstring.Substring(0, i);
            }
            return fullstring;
        }
    }
}