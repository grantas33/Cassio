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
    public class ImageAdapter : BaseAdapter
    {
        Context context;

        public ImageAdapter(Context c)
        {
            context = c;
        }

        public override int Count
        {
            get { return thumbIds.Length; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        // create a new ImageView for each item referenced by the Adapter
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ImageView imageView;

            if (convertView == null)
            {  // if it's not recycled, initialize some attributes
                imageView = new ImageView(context);
                imageView.LayoutParameters = new GridView.LayoutParams(200, 200);
                //imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
                //imageView.SetPadding(50, 50, 50, 50);
            }
            else
            {
                imageView = (ImageView)convertView;
            }

            imageView.SetImageResource(thumbIds[position]);
            return imageView;
        }

  

        // references to our images
        int[] thumbIds = {
        Resource.Drawable.banana, Resource.Drawable.apple, Resource.Drawable.orange,
        Resource.Drawable.slicewatermelon, Resource.Drawable.pear, Resource.Drawable.cucumber,
        Resource.Drawable.tomato, Resource.Drawable.loafofbread, Resource.Drawable.potato,
        Resource.Drawable.buckwheat, Resource.Drawable.pastapng, Resource.Drawable.curd,
        Resource.Drawable.egg, Resource.Drawable.cheesecurdsnack, Resource.Drawable.pizzapng,
        Resource.Drawable.souppng, 


    };
    }
}