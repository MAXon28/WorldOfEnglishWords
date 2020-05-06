using Android.OS;
using Android.Views;

namespace WorldOfEnglishWord.Controllers
{
    public class SettingsActivity : Android.Support.V4.App.Fragment
    {
        private View view;

        public static SettingsActivity NewInstance()
        {
            SettingsActivity settingsActivity = new SettingsActivity();
            return settingsActivity;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.settings_view, container, false);
            return view;
        }
    }
}