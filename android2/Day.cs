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
    class Day
    {
        public DateTime Date { get; set; }
        public int Calories { get; set; }
        public int Carbohydrates { get; set; }
        public int Protein { get; set; }
        public int Fat { get; set; }

        public Day() { }
        public Day (DateTime date, int cal, int carbs, int protein, int fat)
        {
            Date = date;
            Calories = cal;
            Carbohydrates = carbs;
            Protein = protein;
            Fat = fat;
        }
    }
}