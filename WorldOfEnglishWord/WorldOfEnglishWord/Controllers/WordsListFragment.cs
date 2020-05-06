using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using WorldOfEnglishWord.Adapters;
using WorldOfEnglishWord.Translator;

namespace WorldOfEnglishWord.Controllers
{
    public class WordsListFragment : Android.Support.V4.App.Fragment
    {
        private View view;
        private RecyclerView recycler;
        private TranslateWordsAdapter translateWordsAdapter;
        private TranslatorSave saveText;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            translateWordsAdapter = new TranslateWordsAdapter();
            saveText = TranslatorSave.NewInstance();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.words_fragment_view, container, false);
            recycler = view.FindViewById<RecyclerView>(Resource.Id.rv_dictionary_translate);
            LinearLayoutManager layoutManager = new LinearLayoutManager(Application.Context);
            recycler.SetLayoutManager(layoutManager);
            recycler.SetAdapter(translateWordsAdapter);
            return view;
        }
    }
}