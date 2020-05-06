using Android.OS;
using Android.Views;

namespace WorldOfEnglishWord.Controllers
{
    public class TestActivity : Android.Support.V4.App.Fragment
    {
        private View view;

        public static TestActivity NewInstance()
        {
            TestActivity testActivity = new TestActivity();
            return testActivity;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.test_view, container, false);
            return view;
        }
    }
}