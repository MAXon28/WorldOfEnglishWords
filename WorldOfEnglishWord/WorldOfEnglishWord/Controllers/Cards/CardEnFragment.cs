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

namespace WorldOfEnglishWord.Controllers.Cards
{
    class CardEnFragment : Android.Support.V4.App.Fragment
    {
        private string word;
        //private CardsAdapter adapter;
        private TextView textView;
        private View view;

        public CardEnFragment()
        {
        }

        public CardEnFragment(string word)
        {
            this.word = word;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.card_fragment_en_view, container, false);
            textView = view.FindViewById<TextView>(Resource.Id.word_en_fr);

            textView.Text = word;

            return view;
        }
    }
}