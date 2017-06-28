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
    
    public class FoodsRepository : SQLiteConnection
    {
 
        object locker = new object();
        public List<Food> foodlist { get; set; }
        public bool update { get; set; }

        public FoodsRepository(string path) : base(path)
        {
            
            CreateTable<Food>();
            foodlist = Table<Food>().ToList();
            update = false;
        }

        public double GetCarbohydrates()
        {
            double sum = 0;

            foreach(Food food in foodlist)
            {
                sum += (food.Carbohydrates / 100) * food.Grams * food.Multiplier;
            }

            return sum;
        }

        public double GetProtein()
        {
            double sum = 0;

            foreach (Food food in foodlist)
            {
                sum += (food.Protein / 100) * food.Grams * food.Multiplier;
            }

            return sum;
        }

        public double GetFat()
        {
            double sum = 0;

            foreach (Food food in foodlist)
            {
                sum += (food.Fat / 100) * food.Grams * food.Multiplier;
            }

            return sum;
        }

        public void UpdateDatabase()
        {
            if (update == false) return;
            lock (locker)
            {
               this.DeleteAll<Food>();
               // UpdateAll(foodlist);
                InsertAll(foodlist);
                update = false;
                   
            }
        }

        //Get all Foods
        public IEnumerable<Food> GetFoods()
        {
            lock (locker)
            {
                return Table<Food>(); //Careful with ToList calls on databases!
            } 
        }


       public Food GetLast()
        {
           return foodlist[foodlist.Count - 1];
        }

        public void DeleteFood(int position)
        {
            if (foodlist[position].Multiplier > 1) foodlist[position].Multiplier--;
            else foodlist.RemoveAt(position);

            update = true;
        }

        public void DeleteFood(Food food)
        {

            foodlist.Remove(food);

            update = true;
        }

        //Delete last Food
        public void DeleteLast()
        {
            if(foodlist.Count() == 0) { return; }
            if (foodlist[foodlist.Count - 1].Multiplier > 1)
            {
                foodlist[foodlist.Count - 1].Multiplier--;
                
            }
            else
            {
                foodlist.RemoveAt(foodlist.Count - 1);
                
            }
            update = true;
            
        }

        //Add new Food to DB
        public void AddFood(Food food)
        {
 
                if (foodlist.Count() > 0 && foodlist[foodlist.Count -1].Name == food.Name)
                {
                    foodlist[foodlist.Count - 1].Multiplier++;
                   
                    
                 }
                else
                {

                    foodlist.Add(food);
                    
                }
            update = true;
           
        }
    }
}