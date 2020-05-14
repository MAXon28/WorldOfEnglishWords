using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace WorldOfEnglishWord.Adapters
{
    public class ErrorsAdapter : RecyclerView.Adapter
    {
        private List<string> wordsForTest;
        private List<string> wordsUserResponse;
        private List<string> wordsDictionaryResponse;

        public ErrorsAdapter(List<string> wordsForTest, List<string> wordsUserResponse, List<string> wordsDictionaryResponse)
        {
            this.wordsForTest = wordsForTest;
            this.wordsUserResponse = wordsUserResponse;
            this.wordsDictionaryResponse = wordsDictionaryResponse;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ErrorsViewHolder myViewHolder = holder as ErrorsViewHolder;
            myViewHolder.Bind(wordsForTest[position], wordsUserResponse[position], wordsDictionaryResponse[position]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            Context context = parent.Context;
            View view = LayoutInflater.From(context).Inflate(Resource.Layout.error_test_view, parent, false);
            return new ErrorsViewHolder(view);
        }

        public override int ItemCount => wordsForTest.Count;

    }

    class ErrorsViewHolder : RecyclerView.ViewHolder
    {
        private readonly TextView textViewWord;
        private readonly TextView textViewUserResponse;
        private readonly TextView textViewDictionaryResponse;

        public ErrorsViewHolder(View itemView) : base(itemView)
        {
            textViewWord = itemView.FindViewById<TextView>(Resource.Id.tv_word_in_test);
            textViewUserResponse = itemView.FindViewById<TextView>(Resource.Id.tv_user_response);
            textViewDictionaryResponse = itemView.FindViewById<TextView>(Resource.Id.tv_dictionary_response);
        }

        public void Bind(string wordForTest, string wordUserResponse, string wordDictionaryResponse)
        {
            textViewWord.Text = wordForTest;
            if (wordUserResponse.Length == 0)
            {
                textViewUserResponse.Text = "-";
            }
            else
            {
                textViewUserResponse.Text = wordUserResponse;
            }
            textViewDictionaryResponse.Text = wordDictionaryResponse;
        }
    }
}