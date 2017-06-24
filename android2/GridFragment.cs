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
using System.IO;
using SQLite;

namespace android2
{
    public class GridFragment : Android.Support.V4.App.Fragment
    {
            
        private Toast mToast = null;
        private string toastMsg = "";
        private int counter = 1;      

        public GridFragment()
        {

        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.RealGridpage, container, false);

            var gridview = view.FindViewById<GridView>(Resource.Id.gridview);
            gridview.Adapter = new ImageAdapter(view.Context);


            gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                
                switch (args.Position.ToString())
                {
                    case "0":

                        ClickActionProcess("Banana", 136);
                        break;
                    case "1":
                        
                        ClickActionProcess("Apple", 200);
                        break;

                    case "2":
                       
                        ClickActionProcess("Orange", 62);
                        break;

                    case "3":
                      
                        ClickActionProcess("Slice of watermelon", 90);
                        break;

                    case "4":
                       
                        ClickActionProcess("Pear", 100);
                        break;

                    case "5":
                        
                        ClickActionProcess("Cucumber", 150);
                        break;

                    case "6":
                       
                        ClickActionProcess("Tomato", 123);
                        break;

                    case "7":
                        
                        ClickActionProcess("Loaf of bread", 100);
                        break;

                    case "8":
                       
                        ClickActionProcess("Potato", 145);
                        break;

                    case "9":
                      
                        ClickActionProcess("Buckwheat(100g)", 345);
                        break;

                    case "10":
                     
                        ClickActionProcess("Pasta(100g)", 360);
                        break;

                    case "11":
                       
                        ClickActionProcess("Curd(100g)", 98);
                        break;

                    case "12":
                       
                        ClickActionProcess("Egg", 100);
                        break;

                    case "13":
                       
                        ClickActionProcess("Cheese curd snack", 150);
                        break;

                    case "14":
                       
                        ClickActionProcess("Slice of pizza", 115);
                        break;

                    case "15":
                        
                        ClickActionProcess("Chicken soup(100g)", 55);
                        break;

                    default:
                        break;

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

            };
           return view;
        }

        public void ClickActionProcess(string foodname, int calories)
        {
            MainActivity.foodsdb.AddFood(new Food(foodname, calories));
            MainActivity.NutritionEdit.PutString("cal", (int.Parse(MainActivity.localNutritionData.GetString("cal", "0")) + calories).ToString());
            MainActivity.NutritionEdit.Apply();
            //MainActivity.caloriekeeper = (int.Parse(MainActivity.caloriekeeper) + calories).ToString();

        }

        public override void OnPause()

        {
            base.OnPause();
            MainActivity.foodsdb.UpdateDatabase();
        }



    }
}