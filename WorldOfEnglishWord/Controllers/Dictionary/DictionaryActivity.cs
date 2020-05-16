using System;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using WorldOfEnglishWord.Adapters;
using WorldOfEnglishWord.BisnessLogic;
using SearchView = Android.Widget.SearchView;

namespace WorldOfEnglishWord.Controllers.Dictionary
{
    public class DictionaryActivity : Android.Support.V4.App.Fragment
    {
        private WordsLogic wordsLogic;
        private View view;
        private SearchView searchView;
        private RecyclerView recyclerView;
        private DictionaryAdapter dictionaryAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            wordsLogic = WordsLogic.GetInstance();
            dictionaryAdapter = new DictionaryAdapter(wordsLogic);
            wordsLogic.LoadStatistics();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.dictionary_view, container, false);
            searchView = view.FindViewById<SearchView>(Resource.Id.sv_word_search);
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.rv_dictionary_words);

            LinearLayoutManager layoutManager = new LinearLayoutManager(Application.Context);
            recyclerView.SetLayoutManager(layoutManager);

            recyclerView.SetAdapter(dictionaryAdapter);

            var callback = new DictionaryItemTouchHelper(WordsLogic.GetInstance(), dictionaryAdapter, this);
            var itemTouchHelper = new ItemTouchHelper(callback);
            itemTouchHelper.AttachToRecyclerView(recyclerView);

            searchView.QueryTextChange += TextChange;

            return view;
        }

        private void TextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            dictionaryAdapter.Filter.InvokeFilter(e.NewText);
        }

        public override void OnDestroy()
        {
            wordsLogic.SetLists();
            base.OnDestroy();
        }
    }
}