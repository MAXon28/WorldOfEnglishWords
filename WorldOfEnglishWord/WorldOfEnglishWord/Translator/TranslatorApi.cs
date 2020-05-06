using System.Collections;
using System.Net;
using Newtonsoft.Json.Linq;

namespace WorldOfEnglishWord.Translator
{
    static class TranslatorApi
    {
        private static WebClient webTranslatorClient;
        private static readonly string urlTranslate = "https://translate.yandex.net/api/v1.5/tr.json/translate";
        private static readonly string key = "trnsl.1.1.20200312T203703Z.b71f6be639ae905f.0b620ae9f8b9ed24f6f5ba8d5eacd1b310ec0a54";
        private static readonly Hashtable errors;

        static TranslatorApi()
        {
            webTranslatorClient = new WebClient();
            errors = new Hashtable
            {
                { 502, "Неизвестная ошибка! Попробуйте ещё раз!" },
                { 414, "Превышен лимит символов" },
                { 403, "Ошибка доступа к Яндекс Переводчику. Обратитесь к разработчику приложения!" },
                { 400, "Неизвестная ошибка! Попробуйте ещё раз!" },
                { 409, "Ошибка на сервере! Просим прощения за оказанные неудобства. Попробуйте воспользоваться переводчиком чуть позже!" },
                { 504, "Превышен лимит ожидания! Попробуйте ещё раз" },
                { 500, "Ошибка на сервере! Просим прощения за оказанные неудобства. Попробуйте воспользоваться переводчиком чуть позже!" },
                { 404, "Hello, 404! NotFound!" },
                { 413, "Превышен лимит символов" },
                { 408, "Превышен лимит ожидания! Попробуйте ещё раз" },
                { 503, "Сервер временно недоступен! Просим прощения за оказанные неудобства. Попробуйте воспользоваться переводчиком чуть позже!" }
            };
        }

        public static void ZeroRequest()
        {
            TranslateWord("ru-en", "М");
        }

        public static (string, int) TranslateWord(string lang, string word)
        {
            string url = $"{urlTranslate}?key={key}&lang={lang}&text={word}";
            try
            {
                return (GetTextByTranslate(url), 1);
            }
            catch (WebException webException)
            {
                return (GetErrorString(webException), 0);
            }
        }

        public static (string, int) TranslateText(string lang, string text)
        {
            text = lang == "ru-en" ? text.Replace("&", " и ") : text.Replace("&", " and ");
            string url = $"{urlTranslate}?key={key}&lang={lang}&text={text}";
            try
            {
                return (GetTextByTranslate(url), 1);
            }
            catch (WebException webException)
            {
                return (GetErrorString(webException), 0);
            }
        }

        private static string GetTextByTranslate(string url)
        {
            string translateJSON = webTranslatorClient.DownloadString(url);
            JObject jsonString = JObject.Parse(translateJSON);
            string translateText = jsonString["text"].ToString();
            translateText = translateText.Replace("\"", "");
            translateText = translateText.Replace("[", "");
            translateText = translateText.Replace("]", "");
            translateText = translateText.Replace("\n", "");
            return translateText.Remove(0, 2);
        }

        private static string GetErrorString(WebException webException)
        {
            WebExceptionStatus status = webException.Status;
            if (status == WebExceptionStatus.ProtocolError)
            {
                HttpWebResponse httpWebEx = (HttpWebResponse)webException.Response;
                string result = errors[(int)httpWebEx.StatusCode] != null ? errors[(int)httpWebEx.StatusCode].ToString() : "Неизвестная ошибка! Приносим свои извинения! Попробуйте, пожалуйста ещё раз.";
                return errors[(int)httpWebEx.StatusCode].ToString();
            }
            else
            {
                return "Проверьте подключение к интернету!";
            }
        }
    }
}