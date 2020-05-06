using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using WorldOfEnglishWord.Controllers;
using WorldOfEnglishWord.DataLogic;

namespace WorldOfEnglishWord.Adapters
{
    public class DictionaryAdapter : RecyclerView.Adapter
    {
        private WordsLogic wordsLogic;

        public DictionaryAdapter(WordsLogic wordsLogic)
        {
            this.wordsLogic = wordsLogic;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyDictionaryViewHolder myViewHolder = holder as MyDictionaryViewHolder;
            myViewHolder.Bind(position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            Context context = parent.Context;
            View view = LayoutInflater.From(context).Inflate(Resource.Layout.dictionary_word_view, parent, false);
            return new MyDictionaryViewHolder(view, context);
        }

        public override int ItemCount => wordsLogic.GetCount();

        public Filter Filter => FilterHelper.NewInstance(WordsLogic.NewInstance().GetLists().Item1, WordsLogic.NewInstance().GetLists().Item2, this);

        public void SetWords(List<string> filtredRuList, List<string> filtredEnList, string lang)
        {
            if (lang != null)
            {
                wordsLogic.SetLists((filtredRuList, filtredEnList));
            }
            else
            {
                wordsLogic.SetLists();
            }
        }
    }

    class MyDictionaryViewHolder : RecyclerView.ViewHolder
    {
        private readonly TextView textViewRu;
        private readonly TextView textViewEn;
        private readonly WordsLogic wordsLogic;
        private Context context;

        public MyDictionaryViewHolder(View itemView, Context context) : base(itemView)
        {
            textViewRu = itemView.FindViewById<TextView>(Resource.Id.tv_ru);
            textViewEn = itemView.FindViewById<TextView>(Resource.Id.tv_en);
            this.context = context;
            wordsLogic = WordsLogic.NewInstance();
            textViewRu.Click += ItemRu_Click;
            textViewEn.Click += ItemEn_Click;
        }

        public void Bind(int index)
        {
            textViewRu.Text = wordsLogic.GetRuWord(index);
            textViewEn.Text = wordsLogic.GetEnWord(index);
        }

        private void ItemRu_Click(object sender, EventArgs e)
        {
            StartCardActivity("ru");
        }

        private void ItemEn_Click(object sender, EventArgs e)
        {
            StartCardActivity("en");
        }

        private void StartCardActivity(string language)
        {
            var intent = new Intent(context, typeof(CardsActivityPager));
            intent.PutExtra("position", LayoutPosition);
            intent.PutExtra("language", language);
            context.StartActivity(intent);
        }
    }
}