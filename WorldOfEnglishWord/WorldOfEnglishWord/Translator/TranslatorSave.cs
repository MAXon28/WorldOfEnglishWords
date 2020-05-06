using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorldOfEnglishWord.Translator
{
    public class TranslatorSave
    {
        private static TranslatorSave translatorSave;
        private string textForTranslate;
        private string textByTranslate;
        private string countWords;
        private int position;

        public TranslatorSave()
        {
            Task.Run(() => TranslatorApi.ZeroRequest());
            textForTranslate = "";
            textByTranslate = "";
            countWords = "one or null";
            position = 0;
        }

        public static TranslatorSave NewInstance()
        {
            if (translatorSave == null)
            {
                translatorSave = new TranslatorSave();
            }
            return translatorSave;
        }

        public string GetTextForTranslate => textForTranslate;

        public void SetTextForTranslate(string text)
        {
            textForTranslate = text;
        }

        public string GetTextByTranslate => textByTranslate;

        public void SetTextByTranslate(string text)
        {
            textByTranslate = text;
        }

        public string GetCountWords => countWords;

        public void SetCountWords(int count)
        {
            if (count < 2)
            {
                countWords = "one or null";
            }
            else
            {
                countWords = "many";
            }
        }

        public int GetPosition => position;

        public void SetPosition(int number)
        {
            position = number;
        }
    }
}