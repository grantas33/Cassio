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
using System.IO;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data;

namespace android2
{
    
    public class FoodsRepository: Repository<Food> 
    {

        public FoodsRepository(string path) : base(path) { }           

        public override void DeleteData(int position)
        {
            if (datalist[position].Multiplier > 1) datalist[position].Multiplier--;
            else datalist.RemoveAt(position);

            update = true;
        }

        //Add new Food to DB
        public override void AddData(Food data)       
        {
 
                if (datalist.Count() > 0 && datalist[datalist.Count -1].Name == data.Name)
                {
                    datalist[datalist.Count - 1].Multiplier++;
                }
                else
                {
                    datalist.Add(data);                    
                }
            update = true;
           
        }

        public double GetCarbohydrates()
        {
            double sum = 0;

            foreach (Food food in datalist)
            {
                sum += (food.Carbohydrates / 100) * food.Grams * food.Multiplier;
            }

            return sum;
        }

        public double GetProtein()
        {
            double sum = 0;

            foreach (Food food in datalist)
            {
                sum += (food.Protein / 100) * food.Grams * food.Multiplier;
            }

            return sum;
        }

        public double GetFat()
        {
            double sum = 0;

            foreach (Food food in datalist)
            {
                sum += (food.Fat / 100) * food.Grams * food.Multiplier;
            }

            return sum;
        }
    }
}