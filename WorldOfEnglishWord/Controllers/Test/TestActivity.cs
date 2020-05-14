using Android.OS;
using Android.Views;
using Android.Widget;
using WorldOfEnglishWord.Controllers.Test;

namespace WorldOfEnglishWord.Controllers.Test
{
    public class TestActivity : Android.Support.V4.App.Fragment
    {
        private View view;
        private Button buttonGeneration;
        private Button buttonCreating;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.test_view, container, false);
            buttonGeneration = view.FindViewById<Button>(Resource.Id.btn_generation);
            buttonCreating = view.FindViewById<Button>(Resource.Id.btn_creating);

            buttonGeneration.Click += (sender, e) => GenerationTest();
            buttonCreating.Click += (sender, e) => CreatingTest();

            return view;
        }

        public override void OnStart()
        {
            base.OnStart();
            buttonGeneration.Enabled = true;
            buttonCreating.Enabled = true;
        }

        private void GenerationTest()
        {
            buttonGeneration.Enabled = false;
            Activity.StartActivity(typeof(GenerationTestActivity));
        }

        private void CreatingTest()
        {
            buttonCreating.Enabled = false;
            Activity.StartActivity(typeof(CreatingTestActivity));
        }
    }
}