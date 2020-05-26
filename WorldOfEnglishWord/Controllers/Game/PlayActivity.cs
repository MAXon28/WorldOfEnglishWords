using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using WorldOfEnglishWord.BisnessLogic;

namespace WorldOfEnglishWord.Controllers.Game
{
    [Activity(Label = "PlayActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class PlayActivity : Activity
    {
        private GameLogic gameLogic;
        private string[] fullWords;
        private string[] clippedWords;
        private int points;
        private int index;
        private int countAttempts;

        private TextView textViewCountGameAnswers;
        private TextView textViewCountAttempts;
        private TextView textViewWord;
        private EditText editTextInputForWord;
        private Button buttonInput;
        private Button buttonNextWord;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            gameLogic = JsonConvert.DeserializeObject<GameLogic>(Intent.GetStringExtra("GameLogic"));
            (string[], string[]) words = gameLogic.GetWordsForGame();
            fullWords = words.Item1;
            clippedWords = words.Item2;
            index = 0;
            points = 0;
            countAttempts = 5;

            SetContentView(Resource.Layout.play_game_view);

            textViewCountGameAnswers = FindViewById<TextView>(Resource.Id.tv_count_game_answers);
            textViewCountAttempts = FindViewById<TextView>(Resource.Id.tv_count_attempts);
            textViewWord = FindViewById<TextView>(Resource.Id.tv_word_game);
            editTextInputForWord = FindViewById<EditText>(Resource.Id.et_word_game);
            buttonInput = FindViewById<Button>(Resource.Id.btn_input);
            buttonNextWord = FindViewById<Button>(Resource.Id.btn_next_word);

            textViewCountGameAnswers.Text = $"1/{fullWords.Length}";
            textViewCountAttempts.Text = "Осталось попыток: 5";
            textViewWord.Text = clippedWords[0];

            buttonInput.Click += (sender, e) => CheckLetterOrWord();
            buttonNextWord.Click += (sender, e) => GoToTheNextWord();
            editTextInputForWord.KeyPress += (object sender, View.KeyEventArgs e) =>
            {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    CheckLetterOrWord();
                    e.Handled = true;
                }
            };
        }

        public override void OnBackPressed()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            alert.SetTitle("Завершение игры");
            alert.SetMessage("Вы уверены, что хотите завершить игру?");

            alert.SetPositiveButton("Да", (senderAlert, args) =>
            {
                EndGame();
            });

            alert.SetNegativeButton("Нет", (senderAlert, args) =>
            {
            });

            Dialog dialogEndGame = alert.Create();
            dialogEndGame.Show();
        }

        private void CheckLetterOrWord()
        {
            ButtonsSetting(false);

            Toast toast;

            string userInput = editTextInputForWord.Text.ToLower();
            if (userInput.Length == 1)
            {
                if (fullWords[index].Contains(userInput) && !clippedWords[index].Contains(userInput))
                {
                    int countLetterInWord = 0;
                    for (int i = 0; i < fullWords[index].Length; i++)
                    {
                        if (fullWords[index][i] == userInput[0])
                        {
                            clippedWords[index] = clippedWords[index].Substring(0, i) + userInput + clippedWords[index].Substring(i + 1);
                            countLetterInWord++;
                        }
                    }
                    points += 3 * countLetterInWord;
                    textViewWord.Text = clippedWords[index];

                    toast = Toast.MakeText(Application.Context, $"+{3 * countLetterInWord}", ToastLength.Short);
                }
                else
                {
                    toast = Toast.MakeText(Application.Context, "Ошибка!", ToastLength.Short);

                    points -= 1;
                }
                countAttempts--;
                if (clippedWords[index] == fullWords[index] || countAttempts == 0)
                {
                    buttonInput.Visibility = ViewStates.Gone;
                    editTextInputForWord.Visibility = ViewStates.Gone;
                    textViewWord.Text = fullWords[index];
                }
            }
            else if (userInput == fullWords[index])
            {
                if (countAttempts == 5)
                {
                    points += 10;
                    toast = Toast.MakeText(Application.Context, "+10! Молодец!", ToastLength.Long);
                }
                else
                {
                    points += 3;
                    toast = Toast.MakeText(Application.Context, "+3", ToastLength.Short);
                }
                buttonInput.Visibility = ViewStates.Gone;
                editTextInputForWord.Visibility = ViewStates.Gone;

                textViewWord.Text = fullWords[index];
                countAttempts = 0;
            }
            else
            {
                points -= 5;

                toast = Toast.MakeText(Application.Context, "Ошибка! Не сдавайтесь!", ToastLength.Long);
                buttonInput.Visibility = ViewStates.Gone;
                editTextInputForWord.Visibility = ViewStates.Gone;
                textViewWord.Text = fullWords[index];

                countAttempts = 0;
            }

            toast.SetGravity(GravityFlags.Center, 0, -67);
            toast.Show();

            textViewCountAttempts.Text = $"Осталось попыток: {countAttempts}";
            editTextInputForWord.Text = "";
            ButtonsSetting(true);
        }

        private void GoToTheNextWord()
        {
            ButtonsSetting(false);

            if (index != fullWords.Length - 1)
            {
                index++;
                countAttempts = 5;

                textViewCountGameAnswers.Text = $"{index + 1}/{fullWords.Length}";
                textViewCountAttempts.Text = $"Осталось попыток: {countAttempts}";
                textViewWord.Text = clippedWords[index];
                editTextInputForWord.Text = "";

                ButtonsSetting(true);
                buttonInput.Visibility = ViewStates.Visible;
                editTextInputForWord.Visibility = ViewStates.Visible;
                if (index == fullWords.Length - 1)
                {
                    buttonNextWord.Text = "Завершить игру";
                }
            }
            else
            {
                EndGame();
            }
        }

        private void ButtonsSetting(bool isEnabled)
        {
            buttonInput.Enabled = isEnabled;
            buttonNextWord.Enabled = isEnabled;
        }

        private void EndGame()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            alert.SetTitle("Конец игры");
            int units = Convert.ToInt32(points.ToString()[points.ToString().Length - 1].ToString());
            int dozen = 0;
            if (points.ToString().Length > 1)
            {
                dozen = Convert.ToInt32(points.ToString()[points.ToString().Length - 2].ToString());
            }
            if (units == 0 || units >= 5 || dozen == 1)
            {
                alert.SetMessage($"За эту игру Вы набрали {points} очков!");
            }
            else if (units >= 2 && units < 5)
            {
                alert.SetMessage($"За эту игру Вы набрали {points} очка!");
            }
            else
            {
                alert.SetMessage($"За эту игру Вы набрали {points} очко!");
            }
            

            alert.SetPositiveButton("Ок", (senderAlert, args) =>
            {
                gameLogic.Game.AllGames += 1;
                gameLogic.Game.NumberOfPoints += points;
                gameLogic.UpdateDataGameAsync();

                Finish();
            });

            Dialog dialogEndGame = alert.Create();
            dialogEndGame.Show();

            ButtonsSetting(true);
        }
    }
}