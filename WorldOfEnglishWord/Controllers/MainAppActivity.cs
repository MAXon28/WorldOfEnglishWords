using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using WorldOfEnglishWord.Controllers.Dictionary;
using WorldOfEnglishWord.Controllers.Game;
using WorldOfEnglishWord.Controllers.Test;
using WorldOfEnglishWord.Controllers.Translate;

namespace WorldOfEnglishWord.Controllers
{
    [Activity(Label = "MainAppActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    class MainAppActivity : AppCompatActivity
    {
        private Android.Support.V4.App.Fragment view;
        private int id;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            view = null;
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main_app_view);
            var bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.btm_navigation);
            bottomNavigation.NavigationItemSelected += MenuNavigationItemSelected;
            bottomNavigation.SelectedItemId = Resource.Id.translator;
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (id == 0)
            {
                id = Resource.Id.translator;
                view = new TranslatorActivity();
                SupportFragmentManager.BeginTransaction().Add(Resource.Id.content_view, view).Commit();
            }
            else
            {
                SupportFragmentManager.BeginTransaction().Replace(Resource.Id.content_view, view).Commit();
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutInt("id", id);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            id = savedInstanceState.GetInt("id");
        }

        private void MenuNavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            id = e.Item.ItemId;
            LoadView();
        }

        private void LoadView()
        {
            switch (id)
            {
                case Resource.Id.dictionary:
                    view = new DictionaryActivity();
                    break;
                case Resource.Id.translator:
                    view = new TranslatorActivity();
                    break;
                case Resource.Id.test:
                    view = new TestActivity();
                    break;
                case Resource.Id.settings:
                    view = new GameActivity();
                    break;
            }

            if (view == null)
            {
                return;
            }

            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.content_view, view).Commit();
        }
    }
}