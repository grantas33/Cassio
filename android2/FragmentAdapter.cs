using System;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;

namespace android2
{
    class FragmentAdapter : FragmentPagerAdapter
    {
        public FragmentAdapter(Android.Support.V4.App.FragmentManager fm)
            : base(fm)
        {
        }

        public override int Count
        {
            get { return 2; }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            switch (position)
            {
                case 0:
                    return (Android.Support.V4.App.Fragment) new GridFragment();
                case 1:
                    return (Android.Support.V4.App.Fragment) new ListFragments();
                default:
                    return (Android.Support.V4.App.Fragment) new ListFragments();


            }
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            switch (position)
            {
                case 0:
                    return new Java.Lang.String("Fruits and vegetables");
                case 1:
                    return new Java.Lang.String("My foods");
                default:
                    return new Java.Lang.String("NULLABLE");


            }
        }
    }
}