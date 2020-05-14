using System;
using System.Collections.Generic;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Com.ViewPagerIndicator;
using WorldOfEnglishWord.Translator;

namespace WorldOfEnglishWord.Controllers.Translate
{
    public class TranslatorActivity : Fragment
    {
        private View view;
        private ViewPager viewPager;
        private CirclePageIndicator indicator;
        private Adapters.TranslatePagesAdapter pagerAdapter;
        private List<Fragment> list;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.translator_view, container, false);

            viewPager = view.FindViewById<ViewPager>(Resource.Id.vp_fragments);
            indicator = view.FindViewById<CirclePageIndicator>(Resource.Id.indicator);

            indicator.SetViewPager(viewPager);
            list = new List<Fragment>() { new TranslatorFragment() };

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            indicator.SetViewPager(viewPager);
            if (TranslatorSave.NewInstance().GetCountWords == "one or null")
            {
                pagerAdapter = new Adapters.TranslatePagesAdapter(Activity.SupportFragmentManager, list, 1);
            }
            else
            {
                list.Add(new WordsListFragment());
                pagerAdapter = new Adapters.TranslatePagesAdapter(Activity.SupportFragmentManager, list, 2);
            }
            viewPager.Adapter = pagerAdapter;
            viewPager.SetCurrentItem(TranslatorSave.NewInstance().GetPosition, true);
        }

        public override void OnStop()
        {
            base.OnStop();
            TranslatorSave.NewInstance().SetPosition(viewPager.CurrentItem);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            try
            {
                Activity.SupportFragmentManager.BeginTransaction().Remove(pagerAdapter.GetItem(0)).Commit();
                pagerAdapter = TranslatorFragment.GetPagerAdapter;
                int countFragments = pagerAdapter?.Count ?? -1;
                if (countFragments == 2)
                {
                    Activity.SupportFragmentManager.BeginTransaction().Remove(pagerAdapter.GetItem(1)).Commit();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}