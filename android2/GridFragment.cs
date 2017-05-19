using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V4.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace android2
{
    public class GridFragment : Android.Support.V4.App.Fragment
    {
        private Toast mToast = null;
        private string toastMsg = "";
        private int counter = 1;
        private string foodName = null;
        private string startcalories;
        private string startfoodstring;
        //private bool undone = false;

        public GridFragment()
        {

        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.RealGridpage, container, false);

            var gridview = view.FindViewById<GridView>(Resource.Id.gridview);
            gridview.Adapter = new ImageAdapter(view.Context);

            //Button undobutt = view.FindViewById<Button>(Resource.Id.undobutton);
            var localCalorie = Application.Context.GetSharedPreferences("Calorie", FileCreationMode.Private);
            var CalorieEdit = localCalorie.Edit();
            var localFoods = Application.Context.GetSharedPreferences("Foods", FileCreationMode.Private);
            var FoodEdit = localFoods.Edit();
            startcalories = localCalorie.GetString("cal", "0");
            startfoodstring = localFoods.GetString("food", null);

            //undobutt.Click += (sender, e) =>
            //{
            //    if (startcalories == MainActivity.caloriekeeper)
            //    {
            //        if (mToast != null) mToast.Cancel();
            //        mToast = Toast.MakeText(view.Context, "You haven't selected any foods yet!", ToastLength.Short);
            //        mToast.Show();
            //    }
            //    else
            //    {
            //        MainActivity.caloriekeeper = startcalories;
            //        counter = 1;
            //        MainActivity.foodstring.Clear();
            //        MainActivity.foodstring.Append(startfoodstring);
            //        FoodEdit.Remove("food");
            //        if (startfoodstring != null) FoodEdit.PutString("food", MainActivity.foodstring.ToString());
            //        FoodEdit.Apply();


            //        CalorieEdit.PutString("cal", startcalories);
            //        CalorieEdit.Apply();

            //        undone = true;
            //        if (mToast != null) mToast.Cancel();
            //        mToast = Toast.MakeText(view.Context, "All choices removed", ToastLength.Short);
            //        mToast.Show();
            //    }
            //};


            gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {

                switch (args.Position.ToString())
                {
                    case "0":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 136).ToString();
                        MainActivity.foodstring.Append("Banana, 136 cal.;");
                        foodName = "banana";
                        break;

                    case "1":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 200).ToString();
                        MainActivity.foodstring.Append("Apple, 200 cal.;");
                        foodName = "apple";
                        break;

                    case "2":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 62).ToString();
                        MainActivity.foodstring.Append("Orange, 62 cal.;");
                        foodName = "orange";
                        break;

                    case "3":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 90).ToString();
                        MainActivity.foodstring.Append("Slice of watermelon, 90 cal.;");
                        foodName = "slice of watermelon";
                        break;

                    case "4":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 100).ToString();
                        MainActivity.foodstring.Append("Pear, 100 cal.;");
                        foodName = "pear";
                        break;

                    case "5":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 150).ToString();
                        MainActivity.foodstring.Append("Cucumber, 150 cal.;");
                        foodName = "cucumber";
                        break;

                    case "6":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 123).ToString();
                        MainActivity.foodstring.Append("Tomato, 123 cal.;");
                        foodName = "tomato";
                        break;

                    case "7":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 100).ToString();
                        MainActivity.foodstring.Append("Loaf of bread, 100 cal.;");
                        foodName = "loaf of bread";
                        break;

                    case "8":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 145).ToString();
                        MainActivity.foodstring.Append("Potato, 145 cal.;");
                        foodName = "potato";
                        break;

                    case "9":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 345).ToString();
                        MainActivity.foodstring.Append("Buckwheat 100g, 345 cal.;");
                        foodName = "100g buckwheat";
                        break;

                    case "10":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 360).ToString();
                        MainActivity.foodstring.Append("Pasta 100g, 360 cal.;");
                        foodName = "100g pasta";
                        break;

                    case "11":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 98).ToString();
                        MainActivity.foodstring.Append("Curd 100g, 98 cal.;");
                        foodName = "100g curd";
                        break;

                    case "12":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 100).ToString();
                        MainActivity.foodstring.Append("Egg, 100 cal.;");
                        foodName = "egg";
                        break;

                    case "13":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 150).ToString();
                        MainActivity.foodstring.Append("Cheese curd snack, 150 cal.;");
                        foodName = "cheese curd snack";
                        break;

                    case "14":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 115).ToString();
                        MainActivity.foodstring.Append("Slice of pizza, 115 cal.;");
                        foodName = "slice of pizza";
                        break;

                    case "15":
                        MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + 55).ToString();
                        MainActivity.foodstring.Append("Chicken soup 100g, 55 cal.;");
                        foodName = "100g chicken soup";
                        break;

                    default:
                        break;

                }

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
           return view;
        }
    }
}