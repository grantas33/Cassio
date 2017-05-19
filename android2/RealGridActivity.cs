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
using System.Diagnostics;

namespace android2
{
    [Activity(Label = "RealGridActivity")]
    public class RealGridActivity : Activity
    {
        private Toast mToast = null;
        private string toastMsg = "";
        private string prevMsg = "";
        private int counter = 1;
        private string foodName = null;
        private string startcalories;
        private List<string> startfoodstrings = new List<string>();
        private bool undone = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.RealGridpage);

            var gridview = FindViewById<GridView>(Resource.Id.gridview);
            gridview.Adapter = new ImageAdapter(this);

            Button undobutt = FindViewById<Button>(Resource.Id.undobutton);
            var localCalorie = Application.Context.GetSharedPreferences("Calorie", FileCreationMode.Private);
            var CalorieEdit = localCalorie.Edit();
            var localFoods = Application.Context.GetSharedPreferences("Foods", FileCreationMode.Private);
            var FoodEdit = localFoods.Edit();
            startcalories = localCalorie.GetString("cal", "0");
            startfoodstrings = localFoods.GetStringSet("food", startfoodstrings).ToList();

            undobutt.Click += (sender, e) =>
            {
                if (startcalories == MainActivity.caloriekeeper)
                {
                    if (mToast != null) mToast.Cancel();
                    mToast = Toast.MakeText(this, "You haven't selected any foods yet!", ToastLength.Short);
                    mToast.Show();
                }
                else
                {
                    MainActivity.caloriekeeper = startcalories;
                    counter = 1;
                    MainActivity.foodstrings.Clear();
                    MainActivity.foodstrings = startfoodstrings;
                    FoodEdit.PutStringSet("food", MainActivity.foodstrings);
                    FoodEdit.Apply();
                    
                    
                    CalorieEdit.PutString("cal", startcalories);
                    CalorieEdit.Apply();

                    undone = true;
                    if (mToast != null) mToast.Cancel();
                    mToast = Toast.MakeText(this, "All choices removed", ToastLength.Short);
                    mToast.Show();
                }
            };
            

            gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {                        
                prevMsg = toastMsg;

                switch (args.Position.ToString())
                {
                    case "0":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 136).ToString();
                        MainActivity.foodstrings.Add("Banana, 136 cal.");                      
                        foodName = "banana";                 
                        break;

                    case "1":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 200).ToString();
                        MainActivity.foodstrings.Append("Apple, 200 cal.");
                        foodName = "apple";
                        break;

                    case "2":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 62).ToString();
                        MainActivity.foodstrings.Append("Orange, 62 cal.");
                        foodName = "orange";
                        break;

                    case "3":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 90).ToString();
                        MainActivity.foodstrings.Add("Slice of watermelon, 90 cal.");
                        foodName = "slice of watermelon";
                        break;

                    case "4":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 100).ToString();
                        MainActivity.foodstrings.Add("Pear, 100 cal.");
                        foodName = "pear";
                        break;

                    case "5":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 150).ToString();
                        MainActivity.foodstrings.Add("Cucumber, 150 cal.");
                        foodName = "cucumber";
                        break;

                    case "6":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 123).ToString();
                        MainActivity.foodstrings.Add("Tomato, 123 cal.");
                        foodName = "tomato";
                        break;

                    case "7":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 100).ToString();
                        MainActivity.foodstrings.Add("Loaf of bread, 100 cal.");
                        foodName = "loaf of bread";
                        break;

                    case "8":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 145).ToString();
                        MainActivity.foodstrings.Add("Potato, 145 cal.");
                        foodName = "potato";
                        break;

                    case "9":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 345).ToString();
                        MainActivity.foodstrings.Add("Buckwheat 100g, 345 cal.");
                        foodName = "100g buckwheat";
                        break;

                    case "10":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 360).ToString();
                        MainActivity.foodstrings.Add("Pasta 100g, 360 cal.");
                        foodName = "100g pasta";
                        break;

                    case "11":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 98).ToString();
                        MainActivity.foodstrings.Add("Curd 100g, 98 cal.");
                        foodName = "100g curd";
                        break;

                    case "12":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 100).ToString();
                        MainActivity.foodstrings.Add("Egg, 100 cal.");
                        foodName = "egg";
                        break;

                    case "13":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 150).ToString();
                        MainActivity.foodstrings.Add("Cheese curd snack, 150 cal.");
                        foodName = "cheese curd snack";
                        break;

                    case "14":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 115).ToString();
                        MainActivity.foodstrings.Add("Slice of pizza, 115 cal.");
                        foodName = "slice of pizza";
                        break;

                    case "15":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 55).ToString();
                        MainActivity.foodstrings.Add("Chicken soup 100g, 55 cal.");
                        foodName = "100g chicken soup";
                        break;

                    default:
                        break;

                }

                if(prevMsg.Contains(foodName) && undone == false)
                {                   
                    counter++;
                }
                else
                {
                    undone = false;
                    counter = 1;
                }

                toastMsg = string.Format("Added {0} {1}", counter, foodName);
                if (mToast != null) mToast.Cancel();
                mToast = Toast.MakeText(this, toastMsg, ToastLength.Short);
                mToast.Show();
                

                CalorieEdit.PutString("cal", MainActivity.caloriekeeper);
                CalorieEdit.Apply();

                
                FoodEdit.PutStringSet("food", MainActivity.foodstrings);
                FoodEdit.Apply();



            };
        }
    }
}