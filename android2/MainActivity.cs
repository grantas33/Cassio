using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Runtime;
using Android.Content;
using Android.Views;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.IO;
using ZXing.Mobile;
using System.Net.Http;
using System.Xml;
using HtmlAgilityPack;
using System.Net;
using System.Threading.Tasks;

namespace android2
{
    [Activity(Label = "android2", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static string caloriekeeper;

        public static StringBuilder daystring = new StringBuilder();

        public static FoodsRepository foodsdb = new FoodsRepository(Path.Combine(
        System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
        "foodlog.db3"));

        public static FoodsRepository saveddb = new FoodsRepository(Path.Combine(
        System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
        "savedfood.db3"));


        void FoodLogButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(GridActivity));
            StartActivity(intent);
        }

        void ChooseFoodButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(TabActivity));
            StartActivity(intent);
        }

        void SaveAndClearButton_Click(object sender, EventArgs e)
        {
            var localCalorie = Application.GetSharedPreferences("Calorie", FileCreationMode.Private);
            TextView calories = FindViewById<TextView>(Resource.Id.caloriestxt);
            var localDays = Application.Context.GetSharedPreferences("Days", FileCreationMode.Private);

            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Confirmation alert");
            alert.SetMessage("Do you really want to save and clear the data?");
            alert.SetPositiveButton("Yes", (senderAlert, args) => {
                var calorieEdit = localCalorie.Edit();
                foodsdb.foodlist.Clear();
                foodsdb.UpdateDatabase();
                //toast
                if (calories.Text != "0")
                {
                    Toast.MakeText(this, "Saved!", ToastLength.Short).Show();
                    daystring.Append(calories.Text);

                    //putting day calories

                    daystring.Append(string.Format(" cal., {0}.{1}.{2};", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                    var dayEdit = localDays.Edit();
                    dayEdit.PutString("days", daystring.ToString());
                    dayEdit.Apply();
                }
                else { Toast.MakeText(this, "There is nothing to save!", ToastLength.Short).Show(); }

                calories.Text = "0";
                caloriekeeper = "0";
                calorieEdit.PutString("cal", "0");
                calorieEdit.Apply();

            });

            alert.SetNegativeButton("No", (senderAlert, args) => {

            });

            Dialog dialog = alert.Create();
            dialog.Show();

        }
        void DailyViewButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(DayListActivity));
            StartActivity(intent);
        }

        void CreateFoodButton_Click(object sender, EventArgs e)
        {

            CreateFoodDialog dialog = new CreateFoodDialog(this);
            dialog.CreateDialog();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            MobileBarcodeScanner.Initialize(Application);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "Cassio!";

            var localCalorie = Application.GetSharedPreferences("Calorie", FileCreationMode.Private);
            var CalorieEdit = localCalorie.Edit();
            var localDays = Application.Context.GetSharedPreferences("Days", FileCreationMode.Private);

            TextView calories = FindViewById<TextView>(Resource.Id.caloriestxt);
            caloriekeeper = localCalorie.GetString("cal", "0");
            
            Button logbutt = FindViewById<Button>(Resource.Id.logbutton);
            Button gridbutt = FindViewById<Button>(Resource.Id.gridbutton);
            Button clearbutt = FindViewById<Button>(Resource.Id.clearbutton);
            Button dailybutt = FindViewById<Button>(Resource.Id.dailyviewbutton);
            Button manualbutt = FindViewById<Button>(Resource.Id.manualbutton);

            logbutt.Click += FoodLogButton_Click;
            gridbutt.Click += ChooseFoodButton_Click;
            clearbutt.Click += SaveAndClearButton_Click;
            dailybutt.Click += DailyViewButton_Click;
            manualbutt.Click += CreateFoodButton_Click;
        }

        protected override void OnResume()
        {
            base.OnResume();
            MainActivity.foodsdb.UpdateDatabase();
            MainActivity.saveddb.UpdateDatabase();
            TextView calories = FindViewById<TextView>(Resource.Id.caloriestxt);
            calories.Text = caloriekeeper;
        }
    }
}

