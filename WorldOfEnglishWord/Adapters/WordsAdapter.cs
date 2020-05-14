using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using WorldOfEnglishWord.BisnessLogic;

namespace WorldOfEnglishWord.Adapters
{
    public class WordsAdapter : RecyclerView.Adapter
    {
        private GenerateOrCreateTestLogic testLogic;
        private string language;

        public WordsAdapter(GenerateOrCreateTestLogic testLogic, string language)
        {
            this.testLogic = testLogic;
            this.language = language;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyWordsViewHolder myViewHolder = holder as MyWordsViewHolder;
            if(language == "ru")
            {
                myViewHolder.Bind(testLogic.GetDataRuWord(position));
            }
            else
            {
                myViewHolder.Bind(testLogic.GetDataEnWord(position));
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            Context context = parent.Context;
            View view = LayoutInflater.From(context).Inflate(Resource.Layout.test_word_view, parent, false);
            return new MyWordsViewHolder(view, testLogic, language);
        }

        public override int ItemCount => language == "ru" ? testLogic.GetCountRuWords() : testLogic.GetCountEnWords();

        public Filter Filter => WordsFilterHelper.NewInstance(testLogic, language, this);

        public void SetTestLogic(GenerateOrCreateTestLogic testLogic)
        {
            this.testLogic = testLogic;
        }

        public GenerateOrCreateTestLogic GetTestLogic()
        {
            return testLogic;
        }
    }

    class MyWordsViewHolder : RecyclerView.ViewHolder
    {
        private readonly TextView textViewWord;
        private readonly CheckBox checkBoxWord;
        private GenerateOrCreateTestLogic testLogic;
        private string language;

        public MyWordsViewHolder(View itemView, GenerateOrCreateTestLogic testLogic, string language) : base(itemView)
        {
            textViewWord = itemView.FindViewById<TextView>(Resource.Id.tv_word);
            checkBoxWord = itemView.FindViewById<CheckBox>(Resource.Id.cb_word);
            this.testLogic = testLogic;
            this.language = language;

            checkBoxWord.Click += (o, e) => SelectWord();
        }

        public void Bind((string, bool) dataWord)
        {
            textViewWord.Text = dataWord.Item1;
            checkBoxWord.Checked = dataWord.Item2;
        }

        private void SelectWord()
        {
            if (language == "ru")
            {
                testLogic.SetRuWordInTest(LayoutPosition, checkBoxWord.Checked);
            }
            else
            {
                testLogic.SetEnWordInTest(LayoutPosition, checkBoxWord.Checked);
            }
        }
    }
}