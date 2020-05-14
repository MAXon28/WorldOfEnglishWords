using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using Newtonsoft.Json;
using WorldOfEnglishWord.Adapters;
using WorldOfEnglishWord.BisnessLogic;
using SearchView = Android.Widget.SearchView;

namespace WorldOfEnglishWord.Controllers.Test
{
    [Activity(Label = "CreatingTestActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class CreatingTestActivity : Activity
    {
        private SearchView searchViewWords;
        private RecyclerView recyclerViewWords;
        private Button buttonRu;
        private Button buttonEn;
        private Button buttonStartTest;

        private GenerateOrCreateTestLogic testLogic;
        private WordsAdapter wordsAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.creating_test_view);

            testLogic = new GenerateOrCreateTestLogic();

            searchViewWords = FindViewById<SearchView>(Resource.Id.sv_word_search);
            recyclerViewWords = FindViewById<RecyclerView>(Resource.Id.rv_words);
            buttonRu = FindViewById<Button>(Resource.Id.btn_ru);
            buttonEn = FindViewById<Button>(Resource.Id.btn_en);
            buttonStartTest = FindViewById<Button>(Resource.Id.btn_start_creating_test);

            LinearLayoutManager layoutManager = new LinearLayoutManager(this);
            recyclerViewWords.SetLayoutManager(layoutManager);

            wordsAdapter = new WordsAdapter(testLogic, "ru");
            recyclerViewWords.SetAdapter(wordsAdapter);

            buttonRu.Enabled = false;
            buttonRu.SetBackgroundResource(Resource.Drawable.background_ru_disenabled_button);

            searchViewWords.QueryTextChange += (sender, e) => TextChange(e.NewText);
            buttonRu.Click += (sender, e) => SelectRuWords();
            buttonEn.Click += (sender, e) => SelectEnWords();
            buttonStartTest.Click += (sender, e) => StartTest();
        }

        private void TextChange(string searchText)
        {
            wordsAdapter.Filter.InvokeFilter(searchText);
        }

        private void SelectRuWords()
        {
            buttonRu.Enabled = false;
            buttonRu.SetBackgroundResource(Resource.Drawable.background_ru_disenabled_button);
            buttonEn.Enabled = true;
            buttonEn.SetBackgroundResource(Resource.Drawable.background_en_button);

            testLogic = wordsAdapter.GetTestLogic();

            wordsAdapter = new WordsAdapter(testLogic, "ru");
            recyclerViewWords.SetAdapter(wordsAdapter);

            TextChange("");
        }

        private void SelectEnWords()
        {
            buttonRu.Enabled = true;
            buttonRu.SetBackgroundResource(Resource.Drawable.background_ru_button);
            buttonEn.Enabled = false;
            buttonEn.SetBackgroundResource(Resource.Drawable.background_en_disenabled_button);

            testLogic = wordsAdapter.GetTestLogic();

            wordsAdapter = new WordsAdapter(testLogic, "en");
            recyclerViewWords.SetAdapter(wordsAdapter);

            TextChange("");
        }

        private void StartTest()
        {
            buttonStartTest.Enabled = false;
            testLogic = wordsAdapter.GetTestLogic();
            if (testLogic.GetCountSelectWords() == 0)
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
            }
            else
            {
                testLogic.CreateWordsForTest();

                Intent intent = new Intent(this, typeof(SolutionTestActivity));
                intent.PutStringArrayListExtra("WordsForTest", testLogic.GetWordsForTest());

                StartActivity(intent);
                Finish();
            }
        }
    }
}