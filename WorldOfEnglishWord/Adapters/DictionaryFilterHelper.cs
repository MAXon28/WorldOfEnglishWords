using System.Collections;
using System.Collections.Generic;
using Android.Runtime;
using Android.Widget;
using Java.Lang;

namespace WorldOfEnglishWord.Adapters
{
    public class DictionaryFilterHelper : Filter
    {
        private static List<string> currentRuList;
        private static List<string> currentEnList;
        private List<string> filterRuResult;
        private List<string> filterEnResult;
        private static DictionaryAdapter adapter;

        public static DictionaryFilterHelper NewInstance(List<string> currentRuList, List<string> currentEnList, DictionaryAdapter adapter)
        {
            DictionaryFilterHelper.currentRuList = currentRuList;
            DictionaryFilterHelper.currentEnList = currentEnList;
            DictionaryFilterHelper.adapter = adapter;
            return new DictionaryFilterHelper();
        }

        protected override FilterResults PerformFiltering(ICharSequence constraint)
        {
            FilterResults filterResults = new FilterResults();

            if (constraint != null && constraint.Length() > 0)
            {
                string searchText = constraint.ToString().ToLower();
                string langSearch = GetSearchLang(searchText);
                if (langSearch != null)
                {
                    if (langSearch == "ru")
                    {
                        GetSearchRuResult(searchText);
                    }
                    else
                    {
                        GetSearchEnResult(searchText);
                    }
                    return filterResults;
                }
                return filterResults;
            }
            return filterResults;
        }

        protected override void PublishResults(ICharSequence constraint, FilterResults results)
        {
            adapter.SetWords(filterRuResult, filterEnResult, constraint.Length() > 0 ? "" : null);
            adapter.NotifyDataSetChanged();
        }

        private void GetSearchRuResult(string searchText)
        {
            filterRuResult  = new List<string>();
            filterEnResult = new List<string>();

            for (int i = 0; i < currentRuList.Count; i++)
            {
                if (currentRuList[i].ToLower().Contains(searchText))
                {
                    filterRuResult.Add(currentRuList[i]);
                    filterEnResult.Add(currentEnList[i]);
                }
            }
        }

        private void GetSearchEnResult(string searchText)
        {
            filterRuResult = new List<string>();
            filterEnResult = new List<string>();

            for (int i = 0; i < currentEnList.Count; i++)
            {
                if (currentEnList[i].ToLower().Contains(searchText))
                {
                    filterRuResult.Add(currentRuList[i]);
                    filterEnResult.Add(currentEnList[i]);
                }
            }
        }

        private string GetSearchLang(string searchText)
        {
            Hashtable hashtableLang = new Hashtable();

            for (int i = 1072; i <= 1103; i++)
            {
                hashtableLang.Add(i, "ru");
            }
            hashtableLang.Add(1105, "ru");

            for (int i = 97; i <= 122; i++)
            {
                hashtableLang.Add(i, "en");
            }

            string lang = hashtableLang[(int)searchText[0]] != null ? hashtableLang[(int)searchText[0]].ToString() : null;
            if (lang != null)
            {
                for (int i = 1; i < searchText.Length; i++)
                {
                    if (hashtableLang[(int)searchText[i]] != null && lang != hashtableLang[(int)searchText[i]].ToString())
                    {
                        return null;
                    }
                }
                return lang;
            }
            else
            {
                return null;
            }
        }
    }
}