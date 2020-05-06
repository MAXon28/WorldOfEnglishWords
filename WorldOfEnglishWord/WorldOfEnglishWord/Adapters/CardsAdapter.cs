using System;
using Android.Support.V4.App;
using WorldOfEnglishWord.Controllers.Cards;
using WorldOfEnglishWord.DataLogic;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

namespace WorldOfEnglishWord.Adapters
{
    class CardsAdapter : FragmentStatePagerAdapter
    {
        private string language;
        private WordsLogic wordsLogic;
        private int index;

        public CardsAdapter(FragmentManager fm, string language) : base(fm)
        {
            this.language = language;
            wordsLogic = WordsLogic.NewInstance();
            Count = wordsLogic.GetCount();

        }

        public override int Count { get; }

        public override Fragment GetItem(int position)
        {
            index = position;
            if (language == "ru")
            {
                return new CardRuFragment(wordsLogic.GetRuWord(position));
            }

            return new CardEnFragment(wordsLogic.GetEnWord(position));
        }

    }
}