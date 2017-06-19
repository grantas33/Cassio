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
        //attempt to commit
        public static string caloriekeeper;

        public static StringBuilder daystring = new StringBuilder();

        public static FoodsRepository foodsdb = new FoodsRepository(Path.Combine(
        System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
        "foodlog.db3"));
        public static FoodsRepository saveddb = new FoodsRepository(Path.Combine(
        System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
        "savedfood.db3"));



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
            };

            dailybutt.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(DayListActivity));
                StartActivity(intent);
            };

            async void Scan(CreateFoodView createFoodView)
            {
                var scanner = new MobileBarcodeScanner();
                createFoodView.FoodInput.Text = createFoodView.CalInput.Text = createFoodView.GramsInput.Text = "";
                createFoodView.FoodInput.Enabled = true;
                createFoodView.CalInput.Enabled = true;

                var result = await scanner.Scan();
                Food food = new Food();
                ProgressDialog progress = new ProgressDialog(this);
                progress.Indeterminate = true;
                progress.SetProgressStyle(ProgressDialogStyle.Spinner);
                progress.SetMessage("Searching the fridge..");
                progress.SetCancelable(false);

                if (result != null)
                {
                    progress.Show();
                    try
                    {
                        food = await FindFoodDataFromUrl(string.Format("https://app.rimi.lt/entry/{0}", result.Text));
                        //issaugojimas duomenu
                        //skanuoti
                    }
                    catch
                    {
                        progress.Dismiss();
                        Toast.MakeText(this, "Product not found :(", ToastLength.Long).Show();
                    }
                    progress.Dismiss();

                    createFoodView.FoodInput.Text = food.Name;
                    createFoodView.CalInput.Text = food.Calories.ToString();
                    createFoodView.FoodInput.Enabled = false;
                    createFoodView.CalInput.Enabled = false;

                    createFoodView.GramsInput.TextChanged += (so, ea) =>
                    {
                        if (createFoodView.GramsInput.Text != string.Empty)
                        {
                            createFoodView.CalInput.Text = (food.Calories * int.Parse(createFoodView.GramsInput.Text) / 100).ToString();
                            createFoodView.Grams = int.Parse(createFoodView.GramsInput.Text);

                        }
                        else
                        {
                            createFoodView.CalInput.Text = food.Calories.ToString();
                            createFoodView.Grams = 100;
                        }
                    };
                }
            }

            manualbutt.Click += (sender, e) =>
            {
                
                var inputView = LayoutInflater.Inflate(Resource.Layout.CreateFoodAlert, null);
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Create your own food");
                alert.SetView(inputView);
                EditText inputfood = inputView.FindViewById<EditText>(Resource.Id.inputnewfood);
                EditText inputcal = inputView.FindViewById<EditText>(Resource.Id.inputnewcal);
                Button scanbutt = inputView.FindViewById<Button>(Resource.Id.scanbutton);
                TextView graminputtext = inputView.FindViewById<TextView>(Resource.Id.alerttext3);
                EditText graminput = inputView.FindViewById<EditText>(Resource.Id.inputgram);
                int grams = 0;

                CreateFoodView createFoodView = new CreateFoodView(inputfood, inputcal, graminput);

                scanbutt.Click += async (send, ev) => {
                    
                    var scanner = new MobileBarcodeScanner();
                    inputfood.Text = inputcal.Text = graminput.Text = "";
                    inputfood.Enabled = true;
                    inputcal.Enabled = true;

                    var result = await scanner.Scan();
                    Food food = new Food();
                    ProgressDialog progress = new ProgressDialog(this);
                    progress.Indeterminate = true;
                    progress.SetProgressStyle(ProgressDialogStyle.Spinner);
                    progress.SetMessage("Searching the fridge..");
                    progress.SetCancelable(false);
                    
                    if (result != null)
                    {                      
                        progress.Show();
                        try
                        {
                            food = await FindFoodDataFromUrl(string.Format("https://app.rimi.lt/entry/{0}", result.Text));
                            //issaugojimas duomenu
                            //skanuoti
                        }
                        catch
                        {
                            progress.Dismiss();
                            Toast.MakeText(this, "Product not found :(", ToastLength.Long).Show();
                            return;
                        }
                        progress.Dismiss();

                        inputfood.Text = food.Name;
                        inputcal.Text = food.Calories.ToString();
                        inputfood.Enabled = false;
                        inputcal.Enabled = false;
                        graminput.TextChanged += (so, ea) =>
                        {
                                if (graminput.Text != string.Empty)
                                {
                                    inputcal.Text = (food.Calories * int.Parse(graminput.Text) / 100).ToString();
                                    grams = int.Parse(graminput.Text);

                                }
                                else
                                {
                                    inputcal.Text = food.Calories.ToString();
                                    grams = 100;
                                }
                        };
                        }
                    
                };


                alert.SetPositiveButton("Add", (senderAlert, args) => {

                   if (!IsValidNumber(inputcal.Text)) { Toast.MakeText(this, "Input some calories!", ToastLength.Short).Show(); }
                    else if (inputfood.Text == string.Empty || inputfood.Text.Contains( ';' )) { Toast.MakeText(this, "Food input is invalid", ToastLength.Short).Show(); }                  
                    else {
                        if (saveddb.foodlist.Where(foo => String.Equals(foo.Name+foo.Grams, inputfood.Text+grams, StringComparison.OrdinalIgnoreCase)).Count() > 0)
                        {
                            AlertDialog.Builder secondalert = new AlertDialog.Builder(this);
                            secondalert.SetTitle("This food already exists in your database. Overwrite?");
                            secondalert.SetPositiveButton("Yes", (s, ea) =>
                            {
                                saveddb.DeleteFood(saveddb.foodlist.Where(foo => String.Equals(foo.Name+foo.Grams, inputfood.Text+grams, StringComparison.OrdinalIgnoreCase)).FirstOrDefault());
                                saveddb.AddFood(new Food(inputfood.Text, int.Parse(inputcal.Text), grams));
                                saveddb.UpdateDatabase();
                                Toast.MakeText(this, string.Format("Added {0} to your food list", inputfood.Text), ToastLength.Short).Show();

                            });
                            secondalert.SetNegativeButton("No", (s, ea) => { });
                            Dialog seconddialog = secondalert.Create();
                            seconddialog.Show();

                        }
                        else
                        {
                            saveddb.AddFood(new Food(inputfood.Text, int.Parse(inputcal.Text), grams));
                            saveddb.UpdateDatabase();

                            Toast.MakeText(this, string.Format("Added {0} to your food list", inputfood.Text), ToastLength.Short).Show();
                        }
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
           MainActivity.foodsdb.UpdateDatabase();
            MainActivity.saveddb.UpdateDatabase();
            TextView calories = FindViewById<TextView>(Resource.Id.caloriestxt);           
            calories.Text = caloriekeeper;

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

        public async Task<Food> FindFoodDataFromUrl(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            WebResponse myResponse;
            HtmlDocument doc = new HtmlDocument();
            string foodname = "notset";
            int foodcal = 0;
            var food = new Food(foodname, foodcal);

            //try
            //{           
                myResponse = await request.GetResponseAsync(); 
                doc.Load(myResponse.GetResponseStream());
            //}
            //catch
            //{
            //    return food;
            //}           

            if (doc.DocumentNode != null)
            {
                var titlenode = doc.DocumentNode.Descendants("title").FirstOrDefault();
                string titlename = titlenode.InnerHtml;
                foodname = titlename.Substring(0, titlename.Length - 19);
                string numbers = "0123456789";
                StringBuilder sb = new StringBuilder();

                int calind = doc.DocumentNode.OuterHtml.IndexOf(" kcal");
                for (int i = calind-1; i > 0; i--)
                {
                    if (numbers.IndexOf(doc.DocumentNode.OuterHtml[i]) != -1)
                    {
                        sb.Insert(0, doc.DocumentNode.OuterHtml[i]);
                    }
                    else
                    {
                        foodcal = int.Parse(sb.ToString());
                        break;
                    }
                }

                 food = new Food(foodname, foodcal);
                

            }
            else System.Diagnostics.Debug.Write("nullnode");
            return food;
        }
        

    }


}

