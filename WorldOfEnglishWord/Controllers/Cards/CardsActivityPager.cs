using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using WorldOfEnglishWord.Adapters;

namespace WorldOfEnglishWord.Controllers.Cards
{
    [Activity(Label = "CardsActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class CardsActivityPager : FragmentActivity
    {
        private CardsAdapter adapter;
        private ViewPager pager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.card_view);

            var bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.btm_navigation_cards);
            pager = FindViewById<ViewPager>(Resource.Id.vp_words);


            if (Intent.GetStringExtra("language") == "ru")
            {
                adapter = new CardsAdapter(SupportFragmentManager, "ru");
                bottomNavigation.SelectedItemId = Resource.Id.ru_cards;
            }
            else
            {
                adapter = new CardsAdapter(SupportFragmentManager, "en");
                bottomNavigation.SelectedItemId = Resource.Id.en_cards;
            }
            pager.Adapter = adapter;
            pager.SetCurrentItem(Intent.GetIntExtra("position", 0), true);

            bottomNavigation.NavigationItemSelected += MenuNavigationItemSelected;
        }

        private void MenuNavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            int position = pager.CurrentItem;
            switch (e.Item.ItemId)
            {
                case Resource.Id.ru_cards:
                    adapter = new CardsAdapter(SupportFragmentManager, "ru");
                    pager.Adapter = adapter;
                    pager.SetCurrentItem(position, true);
                    break;
                case Resource.Id.en_cards:
                    adapter = new CardsAdapter(SupportFragmentManager, "en");
                    pager.Adapter = adapter;
                    pager.SetCurrentItem(position,true);
                    break;
            }
        }
    }
}