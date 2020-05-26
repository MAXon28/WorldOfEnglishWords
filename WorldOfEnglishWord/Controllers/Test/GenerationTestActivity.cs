using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using WorldOfEnglishWord.BisnessLogic;

namespace WorldOfEnglishWord.Controllers.Test
{
    [Activity(Label = "GenerationTestActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class GenerationTestActivity : Activity
    {
        private RadioButton radioButtonOnlyRu;
        private RadioButton radioButtonAllRu;
        private RadioButton radioButtonOnlyEn;
        private RadioButton radioButtonAllEn;
        private RadioButton radioButtonRandom;
        private RadioButton radioButtonAll;
        private EditText editTextCountWords;
        private Button buttonStartTest;
        private TextView textViewWaiting;

        private GenerateOrCreateTestLogic testLogic;
        private int typeOfGeneration;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            testLogic = new GenerateOrCreateTestLogic();

            SetContentView(Resource.Layout.generation_test_view);

            radioButtonOnlyRu = FindViewById<RadioButton>(Resource.Id.rb_only_ru);
            radioButtonAllRu = FindViewById<RadioButton>(Resource.Id.rb_all_ru);
            radioButtonOnlyEn = FindViewById<RadioButton>(Resource.Id.rb_only_en);
            radioButtonAllEn = FindViewById<RadioButton>(Resource.Id.rb_all_en);
            radioButtonRandom = FindViewById<RadioButton>(Resource.Id.rb_random);
            radioButtonAll = FindViewById<RadioButton>(Resource.Id.rb_all);
            editTextCountWords = FindViewById<EditText>(Resource.Id.et_count_words);
            buttonStartTest = FindViewById<Button>(Resource.Id.btn_start_generation_test);
            textViewWaiting = FindViewById<TextView>(Resource.Id.tv_waiting);

            editTextCountWords.Visibility = Android.Views.ViewStates.Gone;
            buttonStartTest.Enabled = false;
            textViewWaiting.Visibility = Android.Views.ViewStates.Gone;

            radioButtonOnlyRu.Click += (sender, e) => SetOnlyRu();
            radioButtonAllRu.Click += (sender, e) => SetAllRu();
            radioButtonOnlyEn.Click += (sender, e) => SetOnlyEn();
            radioButtonAllEn.Click += (sender, e) => SetAllEn();
            radioButtonRandom.Click += (sender, e) => SetRandom();
            radioButtonAll.Click += (sender, e) => SetAll();
            buttonStartTest.Click += (sender, e) => StartGenerationTest();
        }

        private void SetOnlyRu()
        {
            editTextCountWords.Visibility = Android.Views.ViewStates.Visible;
            typeOfGeneration = 1;
            buttonStartTest.Enabled = true;
        }

        private void SetAllRu()
        {
            editTextCountWords.Visibility = Android.Views.ViewStates.Gone;
            typeOfGeneration = 2;
            buttonStartTest.Enabled = true;
        }

        private void SetOnlyEn()
        {
            editTextCountWords.Visibility = Android.Views.ViewStates.Visible;
            typeOfGeneration = 3;
            buttonStartTest.Enabled = true;
        }

        private void SetAllEn()
        {
            editTextCountWords.Visibility = Android.Views.ViewStates.Gone;
            typeOfGeneration = 4;
            buttonStartTest.Enabled = true;
        }

        private void SetRandom()
        {
            editTextCountWords.Visibility = Android.Views.ViewStates.Visible;
            typeOfGeneration = 5;
            buttonStartTest.Enabled = true;
        }

        private void SetAll()
        {
            editTextCountWords.Visibility = Android.Views.ViewStates.Gone;
            typeOfGeneration = 6;
            buttonStartTest.Enabled = true;
        }

        private void StartGenerationTest()
        {
            buttonStartTest.Enabled = false;
            if (typeOfGeneration == 1 || typeOfGeneration == 3 || typeOfGeneration == 5)
            {
                int countWords = GetNumberInEditText();
                if (countWords <= 0)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Предупреждение");
                    alert.SetMessage("Тест должен состоять минимум из одного слова!");

                    alert.SetPositiveButton("Ок", (senderAlert, args) =>
                    {
                        buttonStartTest.Enabled = true;
                    });

                    Dialog zeroWordsDialog = alert.Create();
                    zeroWordsDialog.Show();
                    zeroWordsDialog.SetCanceledOnTouchOutside(false);
                }
                else
                {
                    textViewWaiting.Visibility = Android.Views.ViewStates.Visible;
                    Task.Run(() => testLogic.GenerateTestByTypeOfGeneration(typeOfGeneration, countWords)).Wait();

                    Intent intent = new Intent(this, typeof(SolutionTestActivity));
                    intent.PutStringArrayListExtra("WordsForTest", testLogic.GetWordsForTest());

                    StartActivity(intent);
                    Finish();
                }
            }
            else
            {
                textViewWaiting.Visibility = Android.Views.ViewStates.Visible;
                Task.Run(() => testLogic.GenerateTestByTypeOfGeneration(typeOfGeneration)).Wait();

                Intent intent = new Intent(this, typeof(SolutionTestActivity));
                intent.PutStringArrayListExtra("WordsForTest", testLogic.GetWordsForTest());

                StartActivity(intent);
                Finish();
            }
        }

        private int GetNumberInEditText()
        {
            try
            {
                return Convert.ToInt32(editTextCountWords.Text);
            }
            catch(Exception)
            {
                return -1;
            }
        }
    }
}