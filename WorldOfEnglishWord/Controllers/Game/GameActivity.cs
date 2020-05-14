using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using WorldOfEnglishWord.BisnessLogic;

namespace WorldOfEnglishWord.Controllers.Game
{
    public class GameActivity : Android.Support.V4.App.Fragment
    {
        private View view;
        private TextView textViewCountGames;
        private TextView textViewNumberOfPoints;
        private Button buttonInformationAboutGame;
        private Button buttonPlayGame;

        private GameLogic gameLogic;
        private int flag;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            gameLogic = new GameLogic();
            flag = 1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.game_view, container, false);

            textViewCountGames = view.FindViewById<TextView>(Resource.Id.tv_count_games);
            textViewNumberOfPoints = view.FindViewById<TextView>(Resource.Id.tv_number_points);
            buttonInformationAboutGame = view.FindViewById<Button>(Resource.Id.btn_information);
            buttonPlayGame = view.FindViewById<Button>(Resource.Id.btn_play);

            buttonInformationAboutGame.Click += (sender, e) => GetInformation();
            buttonPlayGame.Click += (sender, e) => PlayAsync();

            if (WordsLogic.GetInstance().GetEnAllWords().Count == 0)
            {
                buttonPlayGame.Enabled = false;
            }

            return view;
        }

        public override void OnStart()
        {
            base.OnStart();
            if (flag == 1)
            {
                gameLogic.InstallData();
                textViewCountGames.Text = gameLogic.Game.AllGames.ToString();
                textViewNumberOfPoints.Text = gameLogic.Game.NumberOfPoints.ToString();
                flag = 0;
            }
        }

        private void GetInformation()
        {
            buttonInformationAboutGame.Enabled = false;
            buttonPlayGame.Enabled = false;
            AlertDialog.Builder alert = new AlertDialog.Builder(Activity);

            alert.SetTitle("Правила игры");
            alert.SetMessage("Из Вашего словаря автоматически каждую игру выбираются 5 случайных английских слов. В них ставятся от 1 до 4 пропусков.\n" +
                "Ваша задача отгадать пропущенные буквы в словах. За каждую правильно угаданную букву добавляется 3 очка, за неправильную - отнимается 1 очко." +
                "На каждое слово у Вас есть 5 попыток. Также, если Вы сразу написали слово целиком, то за правильный ответ получите +10 очков, если слово окажется неверным - минус пять очков " +
                "(попытки сгорают и отображается верное слово).");

            alert.SetPositiveButton("Ок", (senderAlert, args) =>
            {
                buttonInformationAboutGame.Enabled = true;
                if (WordsLogic.GetInstance().GetEnAllWords().Count != 0)
                {
                    buttonPlayGame.Enabled = true;
                }
            });

            Dialog dialogEndTest = alert.Create();
            dialogEndTest.Show();
        }

        private async void PlayAsync()
        {
            buttonInformationAboutGame.Enabled = false;
            buttonPlayGame.Enabled = false;

            Intent intent = new Intent(Activity, typeof(PlayActivity));
            intent.PutExtra("GameLogic", JsonConvert.SerializeObject(gameLogic));
            StartActivity(intent);

            await Task.Delay(928);
            buttonInformationAboutGame.Enabled = true;
            buttonPlayGame.Enabled = true;

            flag = 1;
        }
    }
}