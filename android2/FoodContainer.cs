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
    public class FoodContainer
    {
        public List<Food> foods { get; set; }

        public FoodContainer()
        {
            foods = new List<Food>();
        }

        public Food GetLast()
        {
            if (foods.Count == 0) return null;
            else return foods[foods.Count - 1];
        }

        public void AddToList(Food food)
        {
            if (foods.Count > 0 && foods[foods.Count-1].Name == food.Name) foods[foods.Count-1].Multiplier++;
            else foods.Add(food);
        }

        public void RemoveLast()
        {
             GetLast().Multiplier--;
            if (GetLast().Multiplier < 1)
            {
                foods.RemoveAt(foods.Count - 1);
            }

        }
    }
}