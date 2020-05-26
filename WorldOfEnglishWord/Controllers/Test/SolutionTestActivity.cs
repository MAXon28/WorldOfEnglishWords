using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using WorldOfEnglishWord.BisnessLogic;

namespace WorldOfEnglishWord.Controllers.Test
{
    [Activity(Label = "SolutionTestActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SolutionTestActivity : Activity
    {
        private TextView textViewCountQuestions;
        private TextView textViewQuestion;
        private EditText editTextAnswers;
        private Button buttonNextQuestion;

        private WordsLogic wordsLogic;
        private List<string> wordsForTest;
        private int countQuestions;
        private int numberOfThisQuestion;
        private int index;

        private List<string> userBadResponse = new List<string>();
        private List<string> dictionaryResponse = new List<string>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            wordsLogic = WordsLogic.GetInstance();

            SetContentView(Resource.Layout.solution_test_view);

            wordsForTest = Intent.GetStringArrayListExtra("WordsForTest").ToList();
            countQuestions = wordsForTest.Count;
            numberOfThisQuestion = 1;
            index = 0;

            textViewCountQuestions = FindViewById<TextView>(Resource.Id.tv_count_answers);
            textViewQuestion = FindViewById<TextView>(Resource.Id.tv_question);
            editTextAnswers = FindViewById<EditText>(Resource.Id.et_answer);
            buttonNextQuestion = FindViewById<Button>(Resource.Id.btn_next_question);

            if (countQuestions == 1)
            {
                buttonNextQuestion.Text = "Завершить тест";
            }
            textViewCountQuestions.Text = $"{numberOfThisQuestion}/{countQuestions}";
            textViewQuestion.Text = wordsForTest[0];

            buttonNextQuestion.Click += (sender, e) => GoToNextQuestion();
        }

        public override void OnBackPressed()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            alert.SetTitle("Завершение теста");
            alert.SetMessage("Вы уверены, что хотите завершить тест?");

            alert.SetPositiveButton("Да", (senderAlert, args) =>
            {
                CompleteTest();

                Intent intent = new Intent(this, typeof(ResultTestActivity));
                intent.PutExtra("CountQuestions", countQuestions);
                intent.PutStringArrayListExtra("WordsForTest", wordsForTest);
                intent.PutStringArrayListExtra("UserResponse", userBadResponse);
                intent.PutStringArrayListExtra("DictionaryResponse", dictionaryResponse);

                StartActivity(intent);
                Finish();
            });

            alert.SetNegativeButton("Нет", (senderAlert, args) =>
            {
            });

            Dialog dialogEndTest = alert.Create();
            dialogEndTest.Show();
        }

        private void GoToNextQuestion()
        {
            buttonNextQuestion.Enabled = false;

            (string, bool) checkResponse = wordsLogic.CheckingResponseToTheTest(wordsForTest[index], editTextAnswers.Text.ToLower());

            if (checkResponse.Item2)
            {
                wordsForTest.Remove(wordsForTest[index]);
            }
            else
            {
                userBadResponse.Add(editTextAnswers.Text);
                dictionaryResponse.Add(checkResponse.Item1);
                index++;
            }

            if (numberOfThisQuestion == countQuestions)
            {
                Intent intent = new Intent(this, typeof(ResultTestActivity));
                intent.PutExtra("CountQuestions", countQuestions);
                intent.PutStringArrayListExtra("WordsForTest", wordsForTest);
                intent.PutStringArrayListExtra("UserResponse", userBadResponse);
                intent.PutStringArrayListExtra("DictionaryResponse", dictionaryResponse);

                StartActivity(intent);
                Finish();
                return;
            }

            numberOfThisQuestion += 1;

            textViewCountQuestions.Text = $"{numberOfThisQuestion}/{countQuestions}";
            textViewQuestion.Text = wordsForTest[index];
            editTextAnswers.Text = "";

            if (numberOfThisQuestion == countQuestions)
            {
                buttonNextQuestion.Text = "Завершить тест";
            }
            buttonNextQuestion.Enabled = true;
        }

        private void CompleteTest()
        {
            while (index != wordsForTest.Count)
            {
                if (editTextAnswers.Text != "")
                {
                    (string, bool) checkResponse = wordsLogic.CheckingResponseToTheTest(wordsForTest[index], editTextAnswers.Text.ToLower());
                    if (checkResponse.Item2)
                    {
                        wordsForTest.Remove(wordsForTest[index]);
                    }
                    else
                    {
                        userBadResponse.Add(editTextAnswers.Text);
                        dictionaryResponse.Add(checkResponse.Item1);
                        index++;
                    }
                    editTextAnswers.Text = "";
                }
                else
                {
                    userBadResponse.Add("");
                    (string, bool) checkResponsePotentialError = wordsLogic.CheckingResponseToTheTest(wordsForTest[index], "");
                    dictionaryResponse.Add(checkResponsePotentialError.Item1);
                    index++;
                }
            }
        }
    }
}