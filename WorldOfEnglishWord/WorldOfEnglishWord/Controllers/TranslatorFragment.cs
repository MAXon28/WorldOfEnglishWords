using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using WorldOfEnglishWord.DataLogic;
using WorldOfEnglishWord.Translator;

namespace WorldOfEnglishWord.Controllers
{
    public class TranslatorFragment : Android.Support.V4.App.Fragment
    {
        private TranslateLogic translateLogic;
        private TranslatorSave saveText;
        private View view;
        private EditText textForTranslate;
        private Button translateButton;
        private TextView textByTranslate;
        private static Adapters.TranslatePagesAdapter pagerAdapter;

        public TranslatorFragment()
        {
            translateLogic = TranslateLogic.NewInstance();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            saveText = TranslatorSave.NewInstance();
        }

        public override void OnPause()
        {
            base.OnPause();
            saveText.SetTextForTranslate(textForTranslate.Text);
            saveText.SetTextByTranslate(textByTranslate.Text);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.translator_fragment_view, container, false);

            textForTranslate = view.FindViewById<EditText>(Resource.Id.txt_for_translate);
            translateButton = view.FindViewById<Button>(Resource.Id.btn_translate);
            textByTranslate = view.FindViewById<TextView>(Resource.Id.txt_by_translate);

            textForTranslate.Text = saveText.GetTextForTranslate;
            textByTranslate.Text = saveText.GetTextByTranslate;

            translateButton.Click += (sender, e) => ClickTranslate();
            textForTranslate.KeyPress += (object sender, View.KeyEventArgs e) =>
            {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    ClickTranslate();
                    e.Handled = true;
                }
            };

            return view;
        }

        private void ClickTranslate()
        {
            textByTranslate.Text = translateLogic.ToTranslate(textForTranslate.Text);

            List<Android.Support.V4.App.Fragment> list = new List<Android.Support.V4.App.Fragment>() { this };
            int count = translateLogic.GetCountWords();
            if (count > 1)
            {
                list.Add(new WordsListFragment());
                pagerAdapter = new Adapters.TranslatePagesAdapter(Activity.SupportFragmentManager, list, 2);
                ViewPager pager = Activity.FindViewById<ViewPager>(Resource.Id.vp_fragments);
                pager.Adapter = pagerAdapter;
                pager.SetCurrentItem(0, true);

                saveText.SetCountWords(count);

                Toast toast = Toast.MakeText(Application.Context, "Листай вправо!", ToastLength.Long);
                toast.Show();
            }
            else
            { 
                pagerAdapter = new Adapters.TranslatePagesAdapter(Activity.SupportFragmentManager, list, 1);
                ViewPager pager = Activity.FindViewById<ViewPager>(Resource.Id.vp_fragments);
                pager.Adapter = pagerAdapter;
                pager.SetCurrentItem(0, true);
                saveText.SetCountWords(count);
            }
        }

        public static Adapters.TranslatePagesAdapter GetPagerAdapter => pagerAdapter;
    }
}