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
    public class Repository<T> : SQLiteConnection where T: new()
    {
        object locker = new object();
        public List<T> datalist { get; set; }
        public bool update { get; set; }

        public Repository(string path) : base(path)
        {
            CreateTable<T>();
            datalist = Table<T>().ToList();
            update = false;
        }

        public void UpdateDatabase()
        {
            if (update == false) return;
            lock (locker)
            {
                this.DeleteAll<T>();
                // UpdateAll(foodlist);
                InsertAll(datalist);
                update = false;

            }
        }

        //Get all data
        public IEnumerable<T> GetData()
        {
            lock (locker)
            {
                return Table<T>(); //Careful with ToList calls on databases!
            }
        }
        

        public T GetLast()
        {
            return datalist[datalist.Count - 1];
        }

        public virtual void DeleteData(int position)
        {
            datalist.RemoveAt(position);
            update = true;
        }

        public void DeleteData(T data)
        {
            datalist.Remove(data);
            update = true;
        }

        public virtual void AddData(T data)
        {
            datalist.Add(data);
            update = true;
        }

    }
}