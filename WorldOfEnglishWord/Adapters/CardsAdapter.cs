using Android.Support.V4.App;
using WorldOfEnglishWord.BisnessLogic;
using WorldOfEnglishWord.Controllers.Cards;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace WorldOfEnglishWord.Adapters
{
    class CardsAdapter : FragmentStatePagerAdapter
    {
        private string language;
        private WordsLogic wordsLogic;

        public CardsAdapter(FragmentManager fm, string language) : base(fm)
        {
            this.language = language;
            wordsLogic = WordsLogic.GetInstance();
            Count = wordsLogic.GetCount();

        }

        public override int Count { get; }

        public override Fragment GetItem(int position)
        {
            if (language == "ru")
            {
                string wordRu = wordsLogic.GetRuWord(position);
                return new CardRuFragment(wordRu, wordsLogic.GetStatisticForWord(wordRu, "ru"));
            }
            string wordEn = wordsLogic.GetEnWord(position);
            return new CardEnFragment(wordEn, wordsLogic.GetStatisticForWord(wordEn, "en"));
        }

    }
}