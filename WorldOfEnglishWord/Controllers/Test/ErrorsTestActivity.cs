using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.Widget;
using WorldOfEnglishWord.Adapters;

namespace WorldOfEnglishWord.Controllers.Test
{
    [Activity(Label = "ErrorsTestActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ErrorsTestActivity : Activity
    {
        private RecyclerView recyclerViewErrorsWords;
        private ErrorsAdapter errorsAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.errors_test_view);

            recyclerViewErrorsWords = FindViewById<RecyclerView>(Resource.Id.rv_errors_statistic);

            LinearLayoutManager layoutManager = new LinearLayoutManager(this);
            recyclerViewErrorsWords.SetLayoutManager(layoutManager);

            errorsAdapter = new ErrorsAdapter(Intent.GetStringArrayListExtra("WordsForTest").ToList(), Intent.GetStringArrayListExtra("UserResponse").ToList(), Intent.GetStringArrayListExtra("DictionaryResponse").ToList());
            recyclerViewErrorsWords.SetAdapter(errorsAdapter);
        }
    }
}