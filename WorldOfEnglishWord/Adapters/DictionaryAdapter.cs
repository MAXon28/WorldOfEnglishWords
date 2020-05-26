using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using WorldOfEnglishWord.BisnessLogic;
using WorldOfEnglishWord.Controllers;
using WorldOfEnglishWord.Controllers.Cards;
using Context = Android.Content.Context;

namespace WorldOfEnglishWord.Adapters
{
    public class DictionaryAdapter : RecyclerView.Adapter
    {
        private WordsLogic wordsLogic;
        private MainAppActivity activity;

        public DictionaryAdapter(WordsLogic wordsLogic, MainAppActivity activity)
        {
            this.wordsLogic = wordsLogic;
            this.activity = activity;
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
            return new MyDictionaryViewHolder(view, context, activity);
        }

        public override int ItemCount => wordsLogic.GetCount();

        public Filter Filter => DictionaryFilterHelper.NewInstance(WordsLogic.GetInstance().GetLists().Item1, WordsLogic.GetInstance().GetLists().Item2, this);

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
        private MainAppActivity activity;

        public MyDictionaryViewHolder(View itemView, Context context, MainAppActivity activity) : base(itemView)
        {
            textViewRu = itemView.FindViewById<TextView>(Resource.Id.tv_ru);
            textViewEn = itemView.FindViewById<TextView>(Resource.Id.tv_en);
            this.context = context;
            this.activity = activity;
            wordsLogic = WordsLogic.GetInstance();
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
            SettingClick(false);
            StartCardActivityAsync("ru");
            Utils.HideKeyboard(activity);
        }

        private void ItemEn_Click(object sender, EventArgs e)
        {
            SettingClick(false);
            StartCardActivityAsync("en");
            Utils.HideKeyboard(activity);
        }

        private async void StartCardActivityAsync(string language)
        {
            var intent = new Intent(context, typeof(CardsActivityPager));
            intent.PutExtra("position", LayoutPosition);
            intent.PutExtra("language", language);
            context.StartActivity(intent);
            await Task.Delay(928);
            SettingClick(true);
        }

        private void SettingClick(bool isEnabled)
        {
            textViewRu.Enabled = isEnabled;
            textViewEn.Enabled = isEnabled;
        }
    }

    public static class Utils
    {
        public static void HideKeyboard(Activity context)
        {
            var imm = (InputMethodManager)context.GetSystemService(Context.InputMethodService);
            try
            {
                imm.HideSoftInputFromWindow(context.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
            }
            catch (Exception)
            {
                // empty
            }
        }
    }
}