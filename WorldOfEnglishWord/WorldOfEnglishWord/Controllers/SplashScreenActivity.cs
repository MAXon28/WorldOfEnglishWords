using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using WorldOfEnglishWord.DataLogic;

namespace WorldOfEnglishWord.Controllers
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "@string/app_name", Icon = "@drawable/ic_launcher", Theme = "@style/AppTheme", MainLauncher = true, NoHistory = true)]
    public class SplashScreenActivity : AppCompatActivity
    {
        private int counter;
        private bool isContinue;
        private Task sleeper;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            counter = 1;
            WordsLogic.NewInstance();

            SetContentView(Resource.Layout.splash_screen_view);
        }

        protected override void OnResume()
        {
            isContinue = true;
            base.OnResume();
            sleeper = new Task(WaitingAsync);
            sleeper.Start();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutInt("counter", counter);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            counter = savedInstanceState.GetInt("counter");
        }

        protected override void OnStop()
        {
            base.OnStop();
            isContinue = false;
        }

        async void WaitingAsync()
        {
            while (counter < 2728)
            {
                if (isContinue)
                {
                    await Task.Delay(1);
                    counter++;
                }
            }
            StartActivity(typeof(MainAppActivity));
            Finish();
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}