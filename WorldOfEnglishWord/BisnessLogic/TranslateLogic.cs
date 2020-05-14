using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorldOfEnglishWord.Translator;
using Console = System.Console;

namespace WorldOfEnglishWord.BisnessLogic
{
    class TranslateLogic
    {
        private static TranslateLogic translateLogic;

        private WordsLogic wordsLogic;

        /// <summary>
        /// помогает определить с какого языка на какой нужно делать перевод
        /// </summary>
        private Hashtable hashtableLang;

        private List<string> wordsForTranslate;

        private List<string> wordsByTranslate;

        /// <summary>
        /// символы, которые помогут разделить текст на слова
        /// </summary>
        private char[] symbols;

        /// <summary>
        /// значение, которое информирует, каким образом был произведён перевод
        /// </summary>
        private string flag;

        private CancellationTokenSource cancelTokenSource;

        private CancellationToken token;

        /// <summary>
        /// подтверждает, что поток завершён, и можно продолжать действия
        /// </summary>
        private bool isReady;

        /// <summary>
        /// отвечает за статус готовности перевода, чтобы завершить поток
        /// </summary>
        private string condition;

        //private delegate void OperationDelegate(string wordForTranslate, string wordByTranslate);
        //private OperationDelegate operationForWord;

        private TranslateLogic()
        {
            wordsLogic = WordsLogic.GetInstance();
            hashtableLang = new Hashtable();

            for (int i = 1072; i <= 1103; i++)
            {
                hashtableLang.Add(i, "ru-en");
            }
            hashtableLang.Add(1105, "ru-en");

            for (int i = 97; i <= 122; i++)
            {
                hashtableLang.Add(i, "en-ru");
            }

            symbols = new char[80];
            for (int i = 0; i < 7; i++)
            {
                symbols[i] = (char)(i + 32);
            }
            for (int i = 7; i < 32; i++)
            {
                symbols[i] = (char)(i + 33);
            }
            for (int i = 32; i < 38; i++)
            {
                symbols[i] = (char)(i + 59);
            }
            for (int i = 38; i < 44; i++)
            {
                symbols[i] = (char)(i + 85);
            }
            for (int i = 44; i < 71; i++)
            {
                symbols[i] = (char)(i + 117);
            }
            for (int i = 71; i < 77; i++)
            {
                symbols[i] = (char)(i + 177);
            }
            symbols[77] = (char)191;
            symbols[78] = (char)215;
            symbols[79] = (char)247;

            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
        }

        public static TranslateLogic NewInstance()
        {
            if (translateLogic == null)
            {
                translateLogic = new TranslateLogic();
            }
            return translateLogic;
        }

        public string ToTranslate(string text)
        {
            flag = "не сохранять";
            condition = "ожидание";
            isReady = false;

            TextAnalysis(text);
            if (wordsForTranslate.Count == 0)
            {
                isReady = true;
                condition = "готово";

                return text;
            }

            if (wordsForTranslate.Count == 1)
            {
                isReady = true;
                condition = "готово";

                string wordByTranslate = OperationForOneWord(wordsForTranslate[0]);
                if (flag == "переведено Яндекс Переводчиком")
                {
                    SendWordToLogic(wordsForTranslate[0].ToLower(), wordByTranslate.ToLower());
                }
                return wordByTranslate;
            }

            return OperationForText(text);
        }

        public int GetCountWords()
        {
            while (isReady == false) ;
            return wordsForTranslate.Count;
        }

        public string GetWordForTranslate(int index)
        {
            return wordsForTranslate[index];
        }

        public string GetWordByTranslate(int index)
        {
            if (wordsByTranslate.Count < index + 1)
            {
                wordsByTranslate.Add(OperationForOneWord(wordsForTranslate[index]));
                Console.WriteLine(wordsForTranslate[index] + " " + wordsByTranslate[index]);
            }
            return wordsByTranslate[index];
        }

        public void UpdateListNewWords(int index)
        {
            SendWordToLogic(wordsForTranslate[index].ToLower(), wordsByTranslate[index].ToLower());
            wordsForTranslate.RemoveAt(index);
            wordsByTranslate.RemoveAt(index);
        }

        private string OperationForOneWord(string word)
        {
            string lang;

            (string, bool) translateInDictionary = GetTranslateInDictionary(word.ToLower());
            if (translateInDictionary.Item2)
            {
                flag = "переведено словарём в приложении";
                return translateInDictionary.Item1;
            }

            lang = GetLangForOneWord(word.ToLower());
            if (lang != null)
            {
                (string, int) translateInformation = TranslatorApi.TranslateWord(lang, word);
                if (translateInformation.Item2 == 1)
                {
                    if (word != translateInformation.Item1)
                    {
                        flag = "переведено Яндекс Переводчиком";
                    }
                }
                else
                {
                    flag = "ошибка";
                }
                return translateInformation.Item1;
            }
            return "Ошибка! Язык не определён!";
        }

        private string OperationForText(string text)
        {
            string lang = GetLangForOneWord(wordsForTranslate[0].ToLower());
            (string, int) translateInformation = TranslatorApi.TranslateText(lang, text);

            if (translateInformation.Item2 == 0)
            {
                cancelTokenSource.Cancel();
            }
            condition = "готово";
            return translateInformation.Item1;
        }

        private (string, bool) GetTranslateInDictionary(string word)
        {
            string wordByTranslate = wordsLogic.GetWordTranslateByApp(word.ToLower());
            if (wordByTranslate != null)
            {
                return (wordByTranslate, true);
            }
            return ("", false);
        }

        private string GetLangForOneWord(string word)
        {
            string lang = hashtableLang[(int)word[0]] != null ? hashtableLang[(int)word[0]].ToString() : null;

            for (int i = 1; i < word.Length; i++)
            {
                if (hashtableLang[(int)word[i]] != null && lang != hashtableLang[(int)word[i]].ToString())
                {
                    return null;
                }
            }
            return lang;
        }

        private void SendWordToLogic(string wordForTranslate, string wordByTranslate)
        {
            Task.Run(() => wordsLogic.SetWordByTranslate(wordForTranslate, wordByTranslate, GetLangForOneWord(wordForTranslate)));
        }

        private void TextAnalysis(string text)
        {
            string[] words = text.Split(symbols);
            wordsForTranslate = (from s in words
                                 where s != ""
                                 select s).ToList();
            if (wordsForTranslate.Count > 1)
            {
                GetAsync();
            }
        }

        private async void GetAsync()
        {
            await Task.Run(() =>
            {
                List<string> wordsList = new List<string>();
                wordsByTranslate = new List<string>();
                foreach (var word in wordsForTranslate)
                {
                    if (wordsList.IndexOf(word.ToLower()) == -1)
                    {
                        wordsList.Add(word.ToLower());
                    }
                }

                while (condition != "готово") ;

                if (token.IsCancellationRequested)
                {
                    wordsForTranslate = new List<string> { wordsForTranslate[0] };
                    isReady = true;
                    return;
                }

                wordsForTranslate = wordsList;
                isReady = true;
            });
        }
    }
}