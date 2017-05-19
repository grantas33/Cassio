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


namespace android2
{
    [Activity(Label = "android2", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static string caloriekeeper;
        public static StringBuilder foodstring = new StringBuilder();
        public static StringBuilder daystring = new StringBuilder();
        public static List<string> savedfoods = new List<string>();
        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
         
      // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "Cassio!";

            var localCalorie = Application.Context.GetSharedPreferences("Calorie", FileCreationMode.Private);
            var CalorieEdit = localCalorie.Edit();
            var localFoods = Application.Context.GetSharedPreferences("Foods", FileCreationMode.Private);
            var localDays = Application.Context.GetSharedPreferences("Days", FileCreationMode.Private);
            var localSavedFoods = Application.Context.GetSharedPreferences("SavedFoods", FileCreationMode.Private);
            foodstring.Clear();
            foodstring.Append(localFoods.GetString("food", null));       
            daystring.Clear();
            daystring.Append(localDays.GetString("days", null));
            savedfoods = localSavedFoods.GetStringSet("savedfood", savedfoods).ToList();


            TextView calories = FindViewById<TextView>(Resource.Id.caloriestxt);
            caloriekeeper = localCalorie.GetString("cal", "0");
            
            Button logbutt = FindViewById<Button>(Resource.Id.logbutton);
            Button gridbutt = FindViewById<Button>(Resource.Id.gridbutton);
            Button clearbutt = FindViewById<Button>(Resource.Id.clearbutton);
            Button dailybutt = FindViewById<Button>(Resource.Id.dailyviewbutton);
            Button manualbutt = FindViewById<Button>(Resource.Id.manualbutton);
           



            logbutt.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(GridActivity));
                StartActivity(intent);
            };

            gridbutt.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(TabActivity));
                StartActivity(intent);
               
            };

            clearbutt.Click += (sender, e) =>
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Confirmation alert");
                alert.SetMessage("Do you really want to save and clear the data?");
                alert.SetPositiveButton("Yes", (senderAlert, args) => {
                    var calorieEdit = localCalorie.Edit();
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
                    var foodEdit = localFoods.Edit();
                    foodEdit.Remove("food");
                    foodstring.Clear();
                    foodEdit.Apply();

                   

                   

                });

                alert.SetNegativeButton("No", (senderAlert, args) => {
                    
                });

                Dialog dialog = alert.Create();
                dialog.Show();
            };

            dailybutt.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(DayListActivity));
                StartActivity(intent);
            };

            manualbutt.Click += (sender, e) =>
            {
                var inputView = LayoutInflater.Inflate(Resource.Layout.CreateFoodAlert, null);
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Create your own food");
                alert.SetView(inputView);
                EditText inputfood = inputView.FindViewById<EditText>(Resource.Id.inputnewfood);
                EditText inputcal = inputView.FindViewById<EditText>(Resource.Id.inputnewcal);


                alert.SetPositiveButton("Add", (senderAlert, args) => {

                   if (!IsValidNumber(inputcal.Text)) { Toast.MakeText(this, "Input some calories!", ToastLength.Short).Show(); }
                    if (inputfood.Text == string.Empty || inputfood.Text.Contains( ';' )) { Toast.MakeText(this, "Food input is invalid", ToastLength.Short).Show(); }
                    {
                        string fooddata = string.Format("{0}, {1} cal.", inputfood.Text, inputcal.Text);
                        savedfoods.Add(fooddata);
                        var savedFoodsEdit = localSavedFoods.Edit();
                        savedFoodsEdit.PutStringSet("savedfood", savedfoods);
                        savedFoodsEdit.Apply();

                        Toast.MakeText(this, string.Format("Added {0} to your food list", inputfood.Text), ToastLength.Short).Show();
                    }
                    
                });

                alert.SetNegativeButton("Cancel", (senderAlert, args) => {

                });

                Dialog dialog = alert.Create();
                dialog.Show();
            };

        }

        protected override void OnResume()
        {
            base.OnResume();
            TextView calories = FindViewById<TextView>(Resource.Id.caloriestxt);           
            calories.Text = caloriekeeper;
           // var localFoods = Application.Context.GetSharedPreferences("Foods", FileCreationMode.Private);

        }

        private bool IsValidNumber(string input)
        {
            string numbers = "0123456789";
            if (input == string.Empty || input[0] == '0' || input.Length > 6) return false;
            for (int i = 0; i < input.Length; i++)
            {
                if (numbers.IndexOf(input[i]) == -1) return false;
            }
            return true;
        }

    }
}

