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
using OxyPlot;
using OxyPlot.Xamarin.Android;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace android2
{
    [Activity(Label = "android2", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        // STATINIAI DUOMENYS

        public static Repository<Day> daysdb = new Repository<Day>(Path.Combine(
        System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
        "daylog.db3"));

        public static ISharedPreferences localNutritionData = Application.Context.GetSharedPreferences("Nutrition", FileCreationMode.Private);
        public static ISharedPreferencesEditor NutritionEdit = localNutritionData.Edit();

        public static FoodsRepository foodsdb = new FoodsRepository(Path.Combine(
        System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
        "foodlog.db3"));

        public static FoodsRepository saveddb = new FoodsRepository(Path.Combine(
        System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
        "savedfood.db3"));
        //STATINIAI DUOMENYS

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
            
            TextView calories = FindViewById<TextView>(Resource.Id.caloriestxt);
            var localDays = Application.Context.GetSharedPreferences("Days", FileCreationMode.Private);

            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Confirmation alert");
            alert.SetMessage("Do you really want to save and clear the data?");
            alert.SetPositiveButton("Yes", (senderAlert, args) => {

                foodsdb.ClearAll();
                foodsdb.UpdateDatabase();
                //toast
                if (calories.Text != "0")
                {
                    Toast.MakeText(this, "Saved!", ToastLength.Short).Show();

                    daysdb.AddData(new Day(DateTime.Now, int.Parse(calories.Text), 0, 0, 0));
                    daysdb.UpdateDatabase();
                }
                else { Toast.MakeText(this, "There is nothing to save!", ToastLength.Short).Show(); }

                calories.Text = "0";                
                NutritionEdit.PutString("cal", "0");
                NutritionEdit.Apply();

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
            UpdateChart();
            ActionBar.Title = "Cassio!";

            var localDays = Application.Context.GetSharedPreferences("Days", FileCreationMode.Private);

            TextView calories = FindViewById<TextView>(Resource.Id.caloriestxt);
            //caloriekeeper = localCalorie.GetString("cal", "0");
            
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

        private void UpdateChart()
        {
            PlotView view = FindViewById<PlotView>(Resource.Id.plot_view);
            view.Model = CreatePlotModel();

        }

        private PlotModel CreatePlotModel()
        {
            var plotModel = new PlotModel { Title = "Nutrients" };
            dynamic series = new PieSeries { StrokeThickness = 2.0, InsideLabelPosition = 0.8, AngleSpan = 360, StartAngle = 0 };
            
            series.Slices.Add(new PieSlice("Fat", foodsdb.GetFat()) { IsExploded = false, Fill = OxyColors.PaleVioletRed });
            series.Slices.Add(new PieSlice("Protein", foodsdb.GetProtein()) { IsExploded = true});
            series.Slices.Add(new PieSlice("Carbohydrates", foodsdb.GetCarbohydrates()) { IsExploded = true });

            plotModel.Series.Add(series);

            return plotModel;
        }

        protected override void OnResume()
        {
            base.OnResume();            
            MainActivity.foodsdb.UpdateDatabase();
            MainActivity.saveddb.UpdateDatabase();
            TextView calories = FindViewById<TextView>(Resource.Id.caloriestxt);
            calories.Text = localNutritionData.GetString("cal", "0");
            UpdateChart();
        }
    }
}

