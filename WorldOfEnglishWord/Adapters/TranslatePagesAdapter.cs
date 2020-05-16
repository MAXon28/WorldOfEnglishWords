using System.Collections.Generic;
using Android.Support.V4.App;
using WorldOfEnglishWord.Controllers;

namespace WorldOfEnglishWord.Adapters
{
    public class TranslatePagesAdapter : FragmentStatePagerAdapter
    {
        private List<Fragment> listFragments;
        private int count;

        public TranslatePagesAdapter(Android.Support.V4.App.FragmentManager fm, List<Fragment> list, int count)
            : base(fm)
        {
            listFragments = list;
            this.count = count;
        }

        public override int Count => count;

        public override Fragment GetItem(int position)
        {
            return listFragments[position];
        }
    }
}