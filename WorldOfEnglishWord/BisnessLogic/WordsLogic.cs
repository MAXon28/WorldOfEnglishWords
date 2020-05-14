using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyDictionary;
using WorldOfEnglishWord.Models;

namespace WorldOfEnglishWord.BisnessLogic
{
    public class WordsLogic
    {
        private static WordsLogic wordsLogic;
        private LogicDb logicDb;
        private DictionaryKeyKey dictionaryWords;
        private List<string> ruAllWords;
        private List<string> enAllWords;
        private List<string> ruListBySearching;
        private List<string> enListBySearching;
        private Dictionary<string, List<int>> ruStatistic;
        private Dictionary<string, List<int>> enStatistic;

        public WordsLogic()
        {
            logicDb = new LogicDb();
            dictionaryWords = new DictionaryKeyKey();
            Task.Run(() => AddToDictionary(logicDb.GetWords()));
        }

        public static WordsLogic GetInstance()
        {
            if (wordsLogic == null)
            {
                wordsLogic = new WordsLogic();
            }
            return wordsLogic;
        }

        public string GetWordTranslateByApp(string word)
        {
            List<string> translateList = dictionaryWords[word];
            if (translateList != null)
            {
                return translateList[new Random().Next(0, translateList.Count)];
            }
            return null;
        }

        public void SetWordByTranslate(string forTranslate, string byTranslate, string lang)
        {
            if (IsAlreadyExist(forTranslate, byTranslate) == false)
            {
                if (lang == "ru-en")
                {
                    ruListBySearching.Add(forTranslate);
                    enListBySearching.Add(byTranslate);
                    dictionaryWords.Add(forTranslate, byTranslate);
                    Task.Run(() => logicDb.Set(forTranslate, byTranslate));
                }
                else
                {
                    ruListBySearching.Add(byTranslate);
                    enListBySearching.Add(forTranslate);
                    dictionaryWords.Add(byTranslate, forTranslate);
                    Task.Run(() => logicDb.Set(byTranslate, forTranslate));
                }
            }
        }

        public void DeleteWordInLists(int index)
        {
            DeleteInDictionary(ruListBySearching[index], enListBySearching[index]);
            int indexRu = ruAllWords.IndexOf(ruListBySearching[index]);
            int indexEn = enAllWords.IndexOf(enListBySearching[index]);
            if (indexRu != indexEn)
            {
                if (ruAllWords[indexRu] == ruAllWords[indexEn])
                {
                    ruAllWords.RemoveAt(indexEn);
                    enAllWords.RemoveAt(indexEn);
                }
                else
                {
                    ruAllWords.RemoveAt(indexRu);
                    enAllWords.RemoveAt(indexRu);
                }
            }
            else
            {
                ruAllWords.RemoveAt(indexRu);
                enAllWords.RemoveAt(indexEn);
            }
            if (ruListBySearching.Count != ruAllWords.Count)
            {
                ruListBySearching.RemoveAt(index);
                enListBySearching.RemoveAt(index);
            }
        }

        public string GetRuWord(int index)
        {
            return ruListBySearching[index];
        }

        public string GetEnWord(int index)
        {
            return enListBySearching[index];
        }

        public List<string> GetRuAllWords()
        {
            return ruAllWords;
        }

        public List<string> GetEnAllWords()
        {
            return enAllWords;
        }

        public int GetCount()
        {
            return ruListBySearching?.Count ?? 0;
        }

        public void SetLists((List<string>, List<string>) newLists)
        {
            ruListBySearching = newLists.Item1;
            enListBySearching = newLists.Item2;
        }

        public void SetLists()
        {
            ruListBySearching = ruAllWords;
            enListBySearching = enAllWords;
        }

        public (List<string>, List<string>) GetLists()
        {
            return (ruAllWords, enAllWords);
        }

        public LogicDb GetLogicDb()
        {
            return logicDb;
        }

        public void LoadStatistics()
        {
            Task[] statisticTasks = {
                new Task(() => ruStatistic = logicDb.GetRuStatistic()),
                new Task(() => enStatistic = logicDb.GetEnStatistic())
            };
            foreach (var task in statisticTasks)
            {
                task.Start();
            }
            Task.WaitAll(statisticTasks);
        }

        public List<int> GetStatisticForWord(string word, string language)
        {
            return language == "ru" ? ruStatistic[word] : enStatistic[word];
        }

        public (string, bool) CheckingResponseToTheTest(string question, string response)
        {
            if (dictionaryWords[question].Contains(response))
            {
                Task.Run(() => logicDb.UpdateStatistic(question, true));
                return (response, true);
            }
            else
            {
                Task.Run(() => logicDb.UpdateStatistic(question, false));
                List<string> words = dictionaryWords[question];
                return (words[new Random().Next(0, words.Count)], false);
            }
        }

        private void AddToDictionary(List<Word> words)
        {
            Task.Run(() => FillingInLists(words));
            foreach (var word in words)
            {
                dictionaryWords.Add(word.RussianVariant.ToLower(), word.EnglishVariant.ToLower());
            }
        }

        private async void FillingInLists(List<Word> words)
        {
            ruListBySearching = new List<string>();
            enListBySearching = new List<string>();
            for (int i = 0; i < words.Count; i++)
            {
                await Task.Run(() => ruListBySearching.Add(words[i].RussianVariant.ToLower()));
                await Task.Run(() => enListBySearching.Add(words[i].EnglishVariant.ToLower()));
            }
            ruAllWords = ruListBySearching;
            enAllWords = enListBySearching;
        }

        private bool IsAlreadyExist(string firstWord, string secondWord)
        {
            List<string> listTranslate = dictionaryWords[firstWord];
            if (listTranslate != null)
            {
                return listTranslate.IndexOf(secondWord) != -1;
            }
            return false;
        }

        private void DeleteInDictionary(string ruWord, string enWord)
        {
            dictionaryWords.Remove(ruWord, enWord);
            Task.Run(() => logicDb.Delete(ruWord, enWord));
        }
    }
}