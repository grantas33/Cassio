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
using SQLite;

namespace android2
{
    [Table ("Foods")]
    public class Food
    {
        

       [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }

        public string Name { get; set; }
        public int Calories { get; set; }
        public int Multiplier { get; set; }
        public int Grams { get; set; }
        public double Carbohydrates { get; set; }
        public double Protein { get; set; }
        public double Fat { get; set; }

        public Food()
        {

        }

        public Food(string name, int calories)
        {
            Name = name;
            Calories = calories;
            Multiplier = 1;
            Grams = 100;            
        }

        public Food(string name, int calories, double car, double pro, double fat)
        {
            Name = name;
            Calories = calories;
            Multiplier = 1;
            Grams = 100;
            Carbohydrates = car;
            Protein = pro;
            Fat = fat;
        }

        public Food(string name, int calories, int grams)
        {
            Name = name;
            Calories = calories;
            Grams = grams;
            Multiplier = 1;
        }

        public Food(string name, int calories, int grams, double car, double pro, double fat)
        {
            Name = name;
            Calories = calories;
            Grams = grams;
            Multiplier = 1;
            Carbohydrates = car;
            Protein = pro;
            Fat = fat;
        }

        public Food(Food food)
        {
            Name = food.Name;
            Calories = food.Calories;
            Multiplier = food.Multiplier;
            Grams = food.Grams;
            Carbohydrates = food.Carbohydrates;
            Protein = food.Protein;
            Fat = food.Fat;
        }




        //public Food(string output)
        //{
        //    string numbers = "0123456789";
        //    StringBuilder sb = new StringBuilder();

        //    if (numbers.IndexOf(output[output.Length - 1]) != -1)
        //    {
        //        for (int i = output.Length - 1; i > 0; i--)
        //        {
        //            if (numbers.IndexOf(output[i]) != -1) sb.Insert(0, output[i]);
        //            else
        //            {
        //                Multiplier = int.Parse(sb.ToString());
        //                output = output.Substring(0, i-1);
        //                break;
        //            }
        //        }
        //    }
        //    else Multiplier = 1;


        //    if(output[output.Length-1] == '.')
        //    {
        //        sb.Clear();
        //        for (int i = output.Length - 6; i > 0; i--)
        //        {
        //            if (numbers.IndexOf(output[i]) != -1) sb.Insert(0, output[i]);
        //            else
        //            {
        //                Calories = int.Parse(sb.ToString());
        //                output = output.Substring(0, i);
        //                break;
        //            }
        //        }
        //    }

        //    for (int i = output.Length-1; i > 0; i--)
        //    {
        //        if (output[i] == ',')
        //        {
        //            Name = output.Substring(0, i);
        //            break;
        //        }
        //    }
        //}

        public override string ToString()
        {
            if(Grams != 0)
            {
                if (Multiplier > 1) return String.Format("{0}({2}g), {1} cal. x{3}\nCarbs:{4}\nProtein:{5}\nFat:{6}", Name, Calories, Grams, Multiplier, Carbohydrates, Protein, Fat);
                else return String.Format("{0}({2}g), {1} cal.\nCarbs:{3}\nProtein:{4}\nFat:{5}", Name, Calories, Grams, Carbohydrates, Protein, Fat);
            }
            if (Multiplier > 1) return String.Format("{0}, {1} cal. x{2}\nCarbs:{3}\nProtein:{4}\nFat:{5}", Name, Calories, Multiplier, Carbohydrates, Protein, Fat);
            else return String.Format("{0}, {1} cal.\nCarbs:{2}\nProtein:{3}\nFat:{4}", Name, Calories, Carbohydrates, Protein, Fat);
        }

        
        
    }
}