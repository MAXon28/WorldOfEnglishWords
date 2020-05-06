using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WorldOfEnglishWord.Models;

namespace WorldOfEnglishWord.Translator
{
    class TranslateLogic
    {
        private WordsLogic wordsLogic;
        private TranslatorApi translatorApi;

        public TranslateLogic()
        {
            wordsLogic = WordsLogic.GetInstance();
            translatorApi = new TranslatorApi();
        }

        public string ToTranslate(string text)
        {
            //GetReadyData(text.ToLower());
            return translatorApi.Translate("ru-en", text);
        }

        public (string, string) GetReadyData(string text)
        {
            string[] words = text.Split(new char[] { ' ', ',', '.', '!', '\n' });
            string firstLang;
            for (int i = 0; i < words.Length; i++)
            {
                for (int j = 0; j < words[i].Length; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        if ((int) words[i][j] >= 97 && (int) words[i][j] <= 122)
                        {
                            firstLang = "en";
                        }
                        else
                        {
                            firstLang = "ru";
                        }
                    }
                }
            }
            return ("", "");
        }
    }
}