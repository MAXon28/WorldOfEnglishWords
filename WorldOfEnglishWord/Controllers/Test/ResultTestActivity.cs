using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;

namespace WorldOfEnglishWord.Controllers.Test
{
    [Activity(Label = "ResultTestActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ResultTestActivity : Activity
    {
        private TextView textViewResult;
        private TextView textViewPercentResult;
        private TextView textViewCountWords;
        private TextView textViewCountCorrectResponse;
        private TextView textViewRatingTest;
        private Button buttonErrors;
        private Button buttonContinue;

        private int countQuestions;
        private List<string> wordsForTest;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.result_test_view);

            countQuestions = Intent.GetIntExtra("CountQuestions", 1);
            wordsForTest = Intent.GetStringArrayListExtra("WordsForTest").ToList();

            textViewResult = FindViewById<TextView>(Resource.Id.tv_result);
            textViewPercentResult = FindViewById<TextView>(Resource.Id.tv_percent_test);
            textViewCountWords = FindViewById<TextView>(Resource.Id.tv_all_words_test);
            textViewCountCorrectResponse = FindViewById<TextView>(Resource.Id.tv_correct_answers_test);
            textViewRatingTest = FindViewById<TextView>(Resource.Id.tv_rating_test);
            buttonErrors = FindViewById<Button>(Resource.Id.btn_errors_in_test);
            buttonContinue = FindViewById<Button>(Resource.Id.btn_continue);

            textViewResult.Text = "Результат";
            textViewPercentResult.Text = GetPercentToTheNearestTenth().ToString() + "%";
            textViewCountWords.Text = countQuestions.ToString();
            textViewCountCorrectResponse.Text = (countQuestions - wordsForTest.Count).ToString();
            textViewRatingTest.Text = GetRating();

            buttonContinue.Click += (sender, e) => CompleteTest();

            if (wordsForTest.Count == 0)
            {
                buttonErrors.Enabled = false;
            }
            else
            {
                buttonErrors.Click += (sender, e) => OpenErrorsAsync();
            }
        }

        private double GetPercentToTheNearestTenth()
        {
            return Math.Round((double)(countQuestions - wordsForTest.Count) / countQuestions * 100, 1, MidpointRounding.AwayFromZero);
        }

        private string GetRating()
        {
            double percent = (double)(countQuestions - wordsForTest.Count) / countQuestions * 100;

            if (percent < 51)
            {
                return "2";
            }

            if (percent < 69)
            {
                return "3";
            }

            if (percent < 86)
            {
                return "4";
            }

            return "5";
        }

        private async void OpenErrorsAsync()
        {
            buttonErrors.Enabled = false;
            buttonContinue.Enabled = false;

            Intent intent = new Intent(this, typeof(ErrorsTestActivity));
            intent.PutStringArrayListExtra("WordsForTest", wordsForTest);
            intent.PutStringArrayListExtra("UserResponse", Intent.GetStringArrayListExtra("UserResponse"));
            intent.PutStringArrayListExtra("DictionaryResponse", Intent.GetStringArrayListExtra("DictionaryResponse"));

            StartActivity(intent);

            await Task.Delay(928);

            buttonErrors.Enabled = true;
            buttonContinue.Enabled = true;
        }

        private void CompleteTest()
        {
            buttonErrors.Enabled = false;
            buttonContinue.Enabled = false;
            Finish();
        }
    }
}