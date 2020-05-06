using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using WorldOfEnglishWord.DataLogic;

namespace WorldOfEnglishWord.Adapters
{
    public class TranslateWordsAdapter : RecyclerView.Adapter
    {
        private readonly TranslateLogic translateLogic;

        public TranslateWordsAdapter()
        {
            translateLogic = TranslateLogic.NewInstance();
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyTranslateViewHolder myViewHolder = holder as MyTranslateViewHolder;
            myViewHolder.Bind(translateLogic.GetWordForTranslate(position), translateLogic.GetWordByTranslate(position));
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            Context context = parent.Context;
            View view = LayoutInflater.From(context).Inflate(Resource.Layout.word_view, parent, false);
            return new MyTranslateViewHolder(view, this, context);
        }

        public override int ItemCount => translateLogic.GetCountWords();
    }

    class MyTranslateViewHolder : RecyclerView.ViewHolder
    {
        private readonly TextView textViewForTranslate;
        private readonly TextView textViewByTranslate;
        private readonly Button saveButton;
        private readonly TranslateWordsAdapter adapter;
        private readonly Context context;

        public MyTranslateViewHolder(View itemView, TranslateWordsAdapter adapter, Context context) : base(itemView)
        {
            textViewForTranslate = itemView.FindViewById<TextView>(Resource.Id.tv_word_for_translate);
            textViewByTranslate = itemView.FindViewById<TextView>(Resource.Id.tv_word_by_translate);
            saveButton = itemView.FindViewById<Button>(Resource.Id.btn_save);
            this.adapter = adapter;
            this.context = context;

            saveButton.Click += (sender, e) => SaveClick(LayoutPosition);
        }

        public void Bind(string wordForTranslate, string wordByTranslate)
        {
            textViewForTranslate.Text = wordForTranslate;
            textViewByTranslate.Text = wordByTranslate;
            saveButton.Visibility = ViewStates.Visible;

            if (wordForTranslate == wordByTranslate || wordByTranslate == "Ошибка! Язык не определён!")
            {
                saveButton.Visibility = ViewStates.Gone;
            }
        }

        private void SaveClick(int position)
        {
            TranslateLogic.NewInstance().UpdateListNewWords(position);
            adapter.NotifyItemRemoved(position);
        }
    }
}