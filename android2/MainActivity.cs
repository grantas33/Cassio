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
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V4;
using Android.Support.V4.Widget;
using static Android.Support.Design.Widget.NavigationView;

namespace android2
{
    [Activity(Label = "android2", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
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

        DrawerLayout drawerLayout;
        NavigationView navigationView;

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

            Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
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

        protected void NavDrawerItem_Select(object sender, NavigationItemSelectedEventArgs e)
        {
            Intent intent = null;
            LinearLayout mainLayout;
            LayoutInflater inflater = null;
            View layout = null;
            e.MenuItem.SetChecked(false);
            switch (e.MenuItem.ItemId)
            {
                case Resource.Id.choose_food:
                    intent = new Intent(this, typeof(TabActivity));
                    StartActivity(intent);
                    break;
                case Resource.Id.create_food:
                    CreateFoodDialog dialog = new CreateFoodDialog(this);
                    dialog.CreateDialog();
                    break;
                case Resource.Id.food_log:
                    intent = new Intent(this, typeof(GridActivity));
                    StartActivity(intent);
                    break;
                case Resource.Id.daily_view:
                    intent = new Intent(this, typeof(DayListActivity));
                    StartActivity(intent);
                    break;
                case Resource.Id.save_clear:
                    SaveAndClearButton_Click(sender, e);
                    break;
            }
            //react to click here and swap fragments or navigate
            drawerLayout.CloseDrawers();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            MobileBarcodeScanner.Initialize(Application);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.DrawerLayout);

            UpdateChart();
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Cassio!";
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu_white_24dp);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.navigation_view);

            ////TODO transfer buttons to Navigation drawer
            navigationView.NavigationItemSelected += NavDrawerItem_Select;

            var localDays = Application.Context.GetSharedPreferences("Days", FileCreationMode.Private);

            TextView calories = FindViewById<TextView>(Resource.Id.caloriestxt);
            //caloriekeeper = localCalorie.GetString("cal", "0");
            FloatingActionButton fabButt = FindViewById<FloatingActionButton>(Resource.Id.fab);
            Button logbutt = FindViewById<Button>(Resource.Id.logbutton);
            Button gridbutt = FindViewById<Button>(Resource.Id.gridbutton);
            Button clearbutt = FindViewById<Button>(Resource.Id.clearbutton);
            Button dailybutt = FindViewById<Button>(Resource.Id.dailyviewbutton);
            Button manualbutt = FindViewById<Button>(Resource.Id.manualbutton);

            fabButt.Click += ChooseFoodButton_Click;
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
            var plotModel = new PlotModel { Title = "Nutrients", TitleColor = OxyColor.FromRgb(255, 255, 255) };
            dynamic series = new PieSeries { StrokeThickness = 2.0, InsideLabelPosition = 0.8, AngleSpan = 360, StartAngle = 0, TextColor = OxyColor.FromRgb(255, 255, 255) };
            series.Slices.Add(new PieSlice("Fat", foodsdb.GetFat()) { IsExploded = false, Fill = OxyColors.PaleVioletRed });
            series.Slices.Add(new PieSlice("Protein", foodsdb.GetProtein()) { IsExploded = true});
            series.Slices.Add(new PieSlice("Carbohydrates", foodsdb.GetCarbohydrates()) { IsExploded = true });

            plotModel.Series.Add(series);

            return plotModel;
        }

        //open nav drawer on home icon press
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch(item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
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

