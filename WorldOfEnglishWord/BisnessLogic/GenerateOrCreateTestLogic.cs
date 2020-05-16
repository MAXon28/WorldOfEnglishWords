using System;
using System.Collections.Generic;
using System.Linq;

namespace WorldOfEnglishWord.BisnessLogic
{
    public class GenerateOrCreateTestLogic
    {
        private WordsLogic wordsLogic;
        private Dictionary<string, bool> ruWords;
        private Dictionary<string, bool> enWords;

        /// <summary>
        /// Для ручного составления теста
        /// </summary>
        private Dictionary<string, bool> ruWordsSearch;
        /// <summary>
        /// Для ручного составления теста
        /// </summary>
        private Dictionary<string, bool> enWordsSearch;

        private int countSelectWords;
        private List<string> wordsForTest;

        public GenerateOrCreateTestLogic()
        {
            wordsLogic = WordsLogic.GetInstance();
            ruWords = new Dictionary<string, bool>();
            enWords = new Dictionary<string, bool>();
            countSelectWords = 0;

            LoadDataRu(wordsLogic.GetRuAllWords());
            LoadDataEn(wordsLogic.GetEnAllWords());
        }

        public (string, bool) GetDataRuWord(int index)
        {
            string word = ruWordsSearch.Keys.ToList()[index];
            bool isSelected = ruWordsSearch[word];
            return (word, isSelected);
        }

        public (string, bool) GetDataEnWord(int index)
        {
            string word = enWordsSearch.Keys.ToList()[index];
            bool isSelected = enWordsSearch[word];
            return (word, isSelected);
        }

        public void SetRuWordInTest(int index, bool itForTest)
        {
            countSelectWords += itForTest ? 1 : -1;
            ruWords[ruWordsSearch.Keys.ToList()[index]] = itForTest;
        }

        public void SetEnWordInTest(int index, bool itForTest)
        {
            countSelectWords += itForTest ? 1 : -1;
            enWords[enWordsSearch.Keys.ToList()[index]] = itForTest;
        }

        public int GetCountRuWords()
        {
            return ruWordsSearch?.Count ?? 0;
        }

        public int GetCountEnWords()
        {
            return enWordsSearch?.Count ?? 0;
        }

        public int GetCountSelectWords()
        {
            return countSelectWords;
        }

        public void GenerateTestByTypeOfGeneration(int typeOfGeneration, int countWords = -1)
        {
            switch (typeOfGeneration)
            {
                case 1:
                    GenerateOnlyRu(countWords);
                    break;
                case 2:
                    GenerateAllRu();
                    break;
                case 3:
                    GenerateOnlyEn(countWords);
                    break;
                case 4:
                    GenerateAllEn();
                    break;
                case 5:
                    GenerateRandom(countWords);
                    break;
                case 6:
                    GenerateAll();
                    break;
            }
        }

        public void CreateWordsForTest()
        {
            wordsForTest = new List<string>();
            foreach (var word in ruWords)
            {
                if (word.Value == true)
                {
                    wordsForTest.Add(word.Key);
                }
            }
            foreach (var word in enWords)
            {
                if (word.Value == true)
                {
                    wordsForTest.Add(word.Key);
                }
            }
        }

        public List<string> GetWordsForTest()
        {
            return wordsForTest;
        }

        public void LoadDataRu(string searchText)
        {
            ruWordsSearch = new Dictionary<string, bool>();
            List<string> list = wordsLogic.GetRuAllWords();
            foreach (var word in list)
            {
                if (!ruWordsSearch.ContainsKey(word) && word.Contains(searchText))
                {
                    ruWordsSearch.Add(word, ruWords[word]);
                }
            }
        }

        public void LoadDataEn(string searchText)
        {
            enWordsSearch = new Dictionary<string, bool>();
            List<string> list = wordsLogic.GetEnAllWords();
            foreach (var word in list)
            {
                if (!enWordsSearch.ContainsKey(word) && word.Contains(searchText))
                {
                    enWordsSearch.Add(word, enWords[word]);
                }
            }
        }

        private void LoadDataRu(List<string> list)
        {
            foreach (var word in list)
            {
                if (!ruWords.ContainsKey(word))
                {
                    ruWords.Add(word, false);
                }
            }
            ruWordsSearch = ruWords;
        }

        private void LoadDataEn(List<string> list)
        {
            foreach (var word in list)
            {
                if (!enWords.ContainsKey(word))
                {
                    enWords.Add(word, false);
                }
            }
            enWordsSearch = enWords;
        }

        private void GenerateOnlyRu(int countWordsInTest)
        {
            if (countWordsInTest >= ruWords.Count)
            {
                GenerateAllRu();
                return;
            }

            wordsForTest = new List<string>();
            int index = 0;
            Random randomKeyIndex = new Random();
            while (index != countWordsInTest)
            {
                string word = ruWords.Keys.ToList()[randomKeyIndex.Next(0, ruWords.Count)];
                if (!wordsForTest.Contains(word))
                {
                    wordsForTest.Add(word);
                    index++;
                }
            }

            countSelectWords = countWordsInTest;
        }

        private void GenerateAllRu()
        {
            wordsForTest = new List<string>();
            for (int i = 0; i < ruWords.Count; i++)
            {
                string word = ruWords.Keys.ToList()[new Random().Next(0, ruWords.Count)];
                if (!wordsForTest.Contains(word))
                {
                    wordsForTest.Add(word);
                }
                else
                {
                    i -= 1;
                }
            }

            countSelectWords = ruWords.Count;
        }

        private void GenerateOnlyEn(int countWordsInTest)
        {
            if (countWordsInTest >= enWords.Count)
            {
                GenerateAllEn();
                return;
            }

            wordsForTest = new List<string>();
            int index = 0;
            Random randomKeyIndex = new Random();
            while (index != countWordsInTest)
            {
                string word = enWords.Keys.ToList()[randomKeyIndex.Next(0, enWords.Count)];
                if (!wordsForTest.Contains(word))
                {
                    wordsForTest.Add(word);
                    index++;
                }
            }

            countSelectWords = countWordsInTest;
        }

        private void GenerateAllEn()
        {
            wordsForTest = new List<string>();
            for (int i = 0; i < enWords.Count; i++)
            {
                string word = enWords.Keys.ToList()[new Random().Next(0, enWords.Count)];
                if (!wordsForTest.Contains(word))
                {
                    wordsForTest.Add(word);
                }
                else
                {
                    i -= 1;
                }
            }

            countSelectWords = enWords.Count;
        }

        private void GenerateRandom(int countWordsInTest)
        {
            if (countWordsInTest >= ruWords.Count + enWords.Count)
            {
                GenerateAll();
                return;
            }
            wordsForTest = new List<string>();
            int index = 0;
            Random random = new Random();
            while (index != countWordsInTest)
            {
                string word = "";
                switch (random.Next(0, 2))
                {
                    case 0:
                        word = ruWords.Keys.ToList()[random.Next(0, ruWords.Count)];
                        break;
                    case 1:
                        word = enWords.Keys.ToList()[random.Next(0, enWords.Count)];
                        break;
                }
                if (!wordsForTest.Contains(word))
                {
                    wordsForTest.Add(word);
                    index++;
                }
            }

            countSelectWords = countWordsInTest;
        }

        private void GenerateAll()
        {
            countSelectWords = ruWords.Count + enWords.Count;
            wordsForTest = new List<string>();
            for (int i = 0; i < countSelectWords; i++)
            {
                string word;
                if (new Random().Next(0, 2) == 0)
                {
                    word = ruWords.Keys.ToList()[new Random().Next(0, ruWords.Count)];
                }
                else
                {
                    word = enWords.Keys.ToList()[new Random().Next(0, enWords.Count)];
                }

                if (!wordsForTest.Contains(word))
                {
                    wordsForTest.Add(word);
                }
                else
                {
                    i -= 1;
                }
            }
        }
    }
}