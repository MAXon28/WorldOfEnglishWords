using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using WorldOfEnglishWord.Adapters;

namespace WorldOfEnglishWord.Controllers.Cards
{
    class CardRuFragment : Android.Support.V4.App.Fragment
    {
        private string word;
        //private CardsAdapter adapter;
        private TextView textView;
        private View view;

        public CardRuFragment()
        {
        }

        public CardRuFragment(string word)
        {
            this.word = word;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.card_fragment_ru_view, container, false);
            textView = view.FindViewById<TextView>(Resource.Id.word_ru_fr);

            textView.Text = word;

            return view;
        }
    }
}