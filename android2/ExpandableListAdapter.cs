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
using Java.Lang;

namespace android2
{
    class ExpandableListAdapter : BaseExpandableListAdapter
    {

        readonly Activity Context;
        
        protected List<Food> FoodList { get; set; }
        bool hasAddition { get; set; }
        
        public ExpandableListAdapter(Activity newContext, List<Food> newList, bool hasaddition) : base()
		{
            Context = newContext;
            FoodList = newList;
            hasAddition = hasaddition;
        }

        
        public override int GroupCount
        {
            get { return FoodList.Count; }
        }

        public override bool HasStableIds
        {
            get { return true; }
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
            //throw new NotImplementedException();
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return 1;
            //throw new NotImplementedException();
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            var item = FoodList[groupPosition];
            View row = convertView;
            if (row == null)
            {
                row = Context.LayoutInflater.Inflate(Resource.Layout.FoodNutritionalInfo, null);
            }

            row.FindViewById<TextView>(Resource.Id.foodinfocarbs).Text = string.Format("Carbohydrates: {0} g", item.Carbohydrates);
            row.FindViewById<TextView>(Resource.Id.foodinfoprotein).Text = string.Format("Protein: {0} g", item.Protein);
            row.FindViewById<TextView>(Resource.Id.foodinfofat).Text = string.Format("Fat: {0} g", item.Fat);



            return row;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
            //throw new NotImplementedException();
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            
            var item = FoodList[groupPosition];
            View view = convertView;
            if (view == null)
            {  
                view = Context.LayoutInflater.Inflate(Resource.Layout.FoodInfo, parent, false);
            }

            if (hasAddition)
            {
                ImageView listHeaderPlusSign = (ImageView)view.FindViewById(Resource.Id.greenplus);
                listHeaderPlusSign.SetImageResource(Resource.Drawable.green_plus);
                listHeaderPlusSign.Visibility = ViewStates.Visible;
               listHeaderPlusSign.Focusable = false;
               listHeaderPlusSign.FocusableInTouchMode = false;
               listHeaderPlusSign.Clickable = false;
               
            }

            ImageView listHeaderArrow = (ImageView)view.FindViewById(Resource.Id.greenarrow);
            int imageResourceId = isExpanded ? Resource.Drawable.green_arrow_up : Resource.Drawable.green_arrow_down;
            listHeaderArrow.SetImageResource(imageResourceId);

            listHeaderArrow.Click += (sender, e) =>
            {
                if (isExpanded) ((ExpandableListView)parent).CollapseGroup(groupPosition);
                else ((ExpandableListView)parent).ExpandGroup(groupPosition, true);
            };




            view.FindViewById<TextView>(Resource.Id.foodinfoname).Text = item.Name;
            view.FindViewById<TextView>(Resource.Id.foodinfocalories).Text = string.Format("{0} cal.", item.Calories.ToString());
            view.FindViewById<TextView>(Resource.Id.foodinfograms).Text = string.Format("{0} g.", item.Grams.ToString());
            view.FindViewById<TextView>(Resource.Id.foodinfocaloriestotal).Text = string.Format("{0} cal.", item.Calories * item.Multiplier);
            view.FindViewById<TextView>(Resource.Id.foodinfogramstotal).Text = string.Format("{0} g.", item.Grams * item.Multiplier);
            view.FindViewById<TextView>(Resource.Id.foodinfomultiplier).Text = "x" + item.Multiplier.ToString();
            if (item.Multiplier > 1)
            {
                view.FindViewById<TextView>(Resource.Id.foodinfomultiplier).Visibility =
                view.FindViewById<TextView>(Resource.Id.foodinfocaloriestotal).Visibility =
                view.FindViewById<TextView>(Resource.Id.foodinfogramstotal).Visibility = ViewStates.Visible;
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.foodinfomultiplier).Visibility =
                view.FindViewById<TextView>(Resource.Id.foodinfocaloriestotal).Visibility =
                view.FindViewById<TextView>(Resource.Id.foodinfogramstotal).Visibility = ViewStates.Gone;
            }

            return view;
        }

       

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return false;
            //throw new NotImplementedException();
        }

        public void UpdateAdapter(List<Food> list)
        {
            this.FoodList = list;
            NotifyDataSetChanged();
        }

    }
}