using System;
using System.Collections.Generic;
using Android.Widget;
using Java.Lang;
using WorldOfEnglishWord.BisnessLogic;

namespace WorldOfEnglishWord.Adapters
{
    public class WordsFilterHelper : Filter
    {
        private static GenerateOrCreateTestLogic testLogic;
        private static string language;
        private static WordsAdapter adapter;
        private GenerateOrCreateTestLogic testLogicUpdate;

        public static WordsFilterHelper NewInstance(GenerateOrCreateTestLogic testLogic, string language, WordsAdapter adapter)
        {
            WordsFilterHelper.testLogic = testLogic;
            WordsFilterHelper.language = language;
            WordsFilterHelper.adapter = adapter;
            return new WordsFilterHelper();
        }

        protected override FilterResults PerformFiltering(ICharSequence constraint)
        {
            FilterResults filterResults = new FilterResults();

            if (constraint != null)
            {
                string searchText = constraint.ToString().ToLower();
                if (language == "ru")
                {
                    testLogic.LoadDataRu(searchText);
                }
                else
                {
                    testLogic.LoadDataEn(searchText);
                }
            }
            testLogicUpdate = testLogic;
            return filterResults;
        }

        protected override void PublishResults(ICharSequence constraint, FilterResults results)
        {
            adapter.SetTestLogic(testLogicUpdate);
            adapter.NotifyDataSetChanged();
        }

    }
}