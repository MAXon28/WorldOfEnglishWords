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
        private List<int> statistic;
        private TextView textViewWord;
        private TextView textViewPercent;
        private TextView textViewAllAnswers;
        private TextView textViewCorrectAnswers;
        private TextView textViewRating;
        private View view;

        public CardEnFragment()
        {
        }

        public CardEnFragment(string word, List<int> statistic)
        {
            this.word = word;
            this.statistic = statistic;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.card_fragment_en_view, container, false);
            textViewWord = view.FindViewById<TextView>(Resource.Id.word_en);
            textViewPercent = view.FindViewById<TextView>(Resource.Id.tv_percent_en);
            textViewAllAnswers = view.FindViewById<TextView>(Resource.Id.tv_all_answers_en);
            textViewCorrectAnswers = view.FindViewById<TextView>(Resource.Id.tv_correct_answers_en);
            textViewRating = view.FindViewById<TextView>(Resource.Id.tv_rating_en);

            textViewWord.Text = word;
            if (statistic[1] != 0)
            {
                textViewPercent.Text = GetPercentToTheNearestTenth().ToString() + "%";
            }
            else
            {
                textViewPercent.Text = "0%";
            }
            textViewAllAnswers.Text = statistic[1].ToString();
            textViewCorrectAnswers.Text = statistic[0].ToString();
            textViewRating.Text = GetRating();

            return view;
        }

        private double GetPercentToTheNearestTenth()
        {
            return Math.Round((double)statistic[0] / statistic[1] * 100, 1, MidpointRounding.AwayFromZero);
        }

        private string GetRating()
        {
            if (statistic[1] < 10)
            {
                return "-";
            }

            float percent = (float)(statistic[0] / statistic[1]) * 100;

            if (percent < 51)
            {
                return "Bad";
            }

            if (percent < 69)
            {
                return "Normal";
            }

            if (percent < 86)
            {
                return "Good";
            }

            return "Excellent";
        }
    }
}