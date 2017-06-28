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
     class DayListAdapter : BaseExpandableListAdapter
    {
        readonly Activity context;
        protected List<Day> datalist;

        public DayListAdapter(Activity newContext, List<Day> newList) : base()
		{
            context = newContext;
            datalist = newList;
        }

        public override int GroupCount
        {
            get
            {
                if (datalist.Count == 0) return 0;
                else return datalist.Select(nn => string.Format("{0}{1}", nn.Date.Year, nn.Date.Month)).Distinct().Count();                 
            }
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
        }

        public override int GetChildrenCount(int groupPosition)
        {
            string text = string.Format("{0}-{1}", datalist[groupPosition].Date.Year, datalist[groupPosition].Date.Month);
            var results = datalist.Where(nn => string.Format("{0}-{1}", nn.Date.Year, nn.Date.Month) == text);
            return results.Count();
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
           
            View row = convertView;
            if (row == null)
            {
                row = context.LayoutInflater.Inflate(Resource.Layout.DayInfoChild, null);
            }
            string text = string.Format("{0}-{1}", datalist[groupPosition].Date.Year, datalist[groupPosition].Date.Month);
            var results = datalist.Where(nn => string.Format("{0}-{1}", nn.Date.Year, nn.Date.Month) == text).ToList();

            row.FindViewById<TextView>(Resource.Id.savedrealdate).Text = results[childPosition].Date.ToShortDateString();
            row.FindViewById<TextView>(Resource.Id.savedcalories).Text = results[childPosition].Calories.ToString() + " cal.";


            return row;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            var item = datalist[groupPosition];
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.DayInfoHeader, parent, false);
            }

            ImageView listHeaderArrow = (ImageView)view.FindViewById(Resource.Id.greenarrow);
            int imageResourceId = isExpanded ? Resource.Drawable.green_arrow_up : Resource.Drawable.green_arrow_down;
            listHeaderArrow.SetImageResource(imageResourceId);

            listHeaderArrow.Click += (sender, e) =>
            {
                if (isExpanded) ((ExpandableListView)parent).CollapseGroup(groupPosition);
                else ((ExpandableListView)parent).ExpandGroup(groupPosition, true);
            };

            view.FindViewById<TextView>(Resource.Id.saveddatetime).Text = string.Format("{0}-{1}", item.Date.Year, item.Date.Month);                     //date

            string text = string.Format("{0}-{1}", datalist[groupPosition].Date.Year, datalist[groupPosition].Date.Month);
            var results = datalist.Where(nn => string.Format("{0}-{1}", nn.Date.Year, nn.Date.Month) == text);
            view.FindViewById<TextView>(Resource.Id.savedaveragecalories).Text = (results.Select(mm => mm.Calories).Sum() / results.Count()).ToString();  //average calories

            return view;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return false;
        }

        public void UpdateAdapter(List<Day> list)
        {
            this.datalist = list;
            NotifyDataSetChanged();
        }
    }
}