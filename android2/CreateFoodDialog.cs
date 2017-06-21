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
using ZXing.Mobile;
using System.Threading.Tasks;
using System.Net;
using HtmlAgilityPack;
using System.IO;

namespace android2
{
    class CreateFoodDialog
    {
        public EditText FoodInput { get; private set; }
        public EditText CalInput { get; private set; }
        public EditText GramsInput { get; private set; }
        public int Grams { get; set; }


        Dialog Dialog { get; set; }
        Context Context { get; set; }
        //FoodsRepository SaveDb { get; set; }

        public CreateFoodDialog(EditText foodInput, EditText calInput, EditText gramsInput, int grams)
        {
            this.FoodInput = foodInput;
            this.CalInput = calInput;
            this.GramsInput = gramsInput;
            this.Grams = grams;

        }

        public CreateFoodDialog(Context context)
        {
            Context = context;
            Grams = 100;         //default value
            //SaveDb = MainActivity.saveddb;
        }

        public void CreateDialog()
        {
            Dialog = new Dialog(Context);
            Dialog.SetContentView(Resource.Layout.CreateFoodAlert);

            Button scanbutt = Dialog.FindViewById<Button>(Resource.Id.scanbutton);
            Button cancelBtn = Dialog.FindViewById<Button>(Resource.Id.cancelBtn);
            Button addBtn = Dialog.FindViewById<Button>(Resource.Id.addBtn);

            FoodInput = Dialog.FindViewById<EditText>(Resource.Id.inputnewfood);
            CalInput = Dialog.FindViewById<EditText>(Resource.Id.inputnewcal);
            GramsInput = Dialog.FindViewById<EditText>(Resource.Id.inputgram);

            scanbutt.Click += Scan;

            cancelBtn.Click += (object sender, EventArgs e) =>
            {
                Dialog.Dismiss();
            };

            addBtn.Click += Add;
            Dialog.Show();

        }

        void Add(object sender, EventArgs e)
        {
            if (!IsValidNumber(CalInput.Text))
            {
                Toast.MakeText(Context, "Food without calories??", ToastLength.Short).Show();
            }
            else if (FoodInput.Text == string.Empty || FoodInput.Text.Contains(';'))
            {
                Toast.MakeText(Context, "Food input is invalid", ToastLength.Short).Show();
            }
            else
            {
                if (MainActivity.saveddb.foodlist.Where(foo => String.Equals(foo.Name, FoodInput.Text, StringComparison.OrdinalIgnoreCase)).Count() > 0)
                {
                    AlertDialog.Builder secondalert = new AlertDialog.Builder(Context);
                    secondalert.SetTitle("This food already exists in your database. Overwrite?");
                    secondalert.SetPositiveButton("Yes", (s, ea) =>
                    {
                        MainActivity.saveddb.DeleteFood(MainActivity.saveddb.foodlist.Where(foo => String.Equals(foo.Name, FoodInput.Text, StringComparison.OrdinalIgnoreCase)).FirstOrDefault());
                        if (GramsInput.Text != "")
                        {
                            Grams = int.Parse(GramsInput.Text);
                        }                        
                        MainActivity.saveddb.AddFood(new Food(FoodInput.Text, int.Parse(CalInput.Text), Grams));
                        MainActivity.saveddb.UpdateDatabase();
                        Toast.MakeText(Context, string.Format("Added {0} to your food list", FoodInput.Text), ToastLength.Short).Show();
                        Dialog.Dismiss();
                    });
                    secondalert.SetNegativeButton("No", (s, ea) => { });
                    Dialog seconddialog = secondalert.Create();
                    seconddialog.Show();

                }
                else
                {
                    if (GramsInput.Text != string.Empty)
                    {
                        Grams = int.Parse(GramsInput.Text);
                    }                    
                    MainActivity.saveddb.AddFood(new Food(FoodInput.Text, int.Parse(CalInput.Text), Grams));
                    MainActivity.saveddb.UpdateDatabase();
                    Dialog.Dismiss();
                    Toast.MakeText(Context, string.Format("Added {0} to your food list", FoodInput.Text), ToastLength.Short).Show();
                }
            }
        }



        async void Scan(object sender, EventArgs e)
        {
            //FoodInput.Text = CalInput.Text = GramsInput.Text = "";
            //FoodInput.Enabled = true;
            //CalInput.Enabled = true;
            var scanner = new MobileBarcodeScanner();

            var result = await scanner.Scan();
            Food food = new Food();
            ProgressDialog progress = new ProgressDialog(Context);
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
                    Toast.MakeText(Context, "Product not found :(", ToastLength.Long).Show();
                }
                progress.Dismiss();

                FoodInput.Text = food.Name;
                CalInput.Text = food.Calories.ToString();
                FoodInput.Enabled = false;
                CalInput.Enabled = false;

                GramsInput.TextChanged += (so, ea) =>
                {
                    if (GramsInput.Text != string.Empty)
                    {
                        CalInput.Text = (food.Calories * int.Parse(GramsInput.Text) / 100).ToString();
                        Grams = int.Parse(GramsInput.Text);

                    }
                    else
                    {
                        CalInput.Text = food.Calories.ToString();
                        Grams = 100;
                    }
                };
            }

            //if (repeat)
            //{
            //    Scan(createFoodView, repeat);
            //}
        }

        public async Task<Food> FindFoodDataFromUrl(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            WebResponse myResponse;
            HtmlDocument doc = new HtmlDocument();
            string foodname = "notset";
            int foodcal = 0;
            var food = new Food(foodname, foodcal);

            myResponse = await request.GetResponseAsync();
            doc.Load(myResponse.GetResponseStream());

            if (doc.DocumentNode != null)
            {
                var titlenode = doc.DocumentNode.Descendants("title").FirstOrDefault();
                string titlename = titlenode.InnerHtml;
                foodname = titlename.Substring(0, titlename.Length - 19);
                string numbers = "0123456789";
                StringBuilder sb = new StringBuilder();

                int calind = doc.DocumentNode.OuterHtml.IndexOf(" kcal");
                for (int i = calind - 1; i > 0; i--)
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

        //protected void OnResume()
        //{
        //    //base.OnResume();
        //    MainActivity.foodsdb.UpdateDatabase();
        //    MainActivity.saveddb.UpdateDatabase();
        //    TextView calories = FindViewById<TextView>(Resource.Id.caloriestxt);
        //    calories.Text = caloriekeeper;

        //}
    }
}