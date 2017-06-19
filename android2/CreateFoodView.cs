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
    class CreateFoodView
    {
        public EditText FoodInput { get; private set; }
        public EditText CalInput { get; private set; }
        public EditText GramsInput { get; private set; }
        public int Grams { get; set; }

        public CreateFoodView(EditText foodInput, EditText calInput, EditText gramsInput)
        {
            FoodInput = foodInput;
            CalInput = CalInput;
            GramsInput = gramsInput;
            Grams = 0;
        }


    }
}