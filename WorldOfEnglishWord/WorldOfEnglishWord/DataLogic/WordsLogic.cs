using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Android.Runtime;
using Android.Speech.Tts;
using MyDictionary;
using WorldOfEnglishWord.Models;

namespace WorldOfEnglishWord.DataLogic
{
    public class WordsLogic
    {
        private static WordsLogic wordsLogic;
        private LogicDb logicDb;
        private DictionaryKeyKey dictionaryWords;
        private List<string> ruAllWords;
        private List<string> enAllWords;
        private List<string> ruList;
        private List<string> enList;
        private Dictionary<string, List<int>> ruStatistic;
        private Dictionary<string, List<int>> enStatistic;

        public WordsLogic()
        {
            logicDb = new LogicDb();
            dictionaryWords = new DictionaryKeyKey();
            Task.Run(() => AddToDictionary(logicDb.GetWords()));
        }

        public static WordsLogic NewInstance()
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
                    SetRuWord(forTranslate);
                    SetEnWord(byTranslate);
                    dictionaryWords.Add(forTranslate, byTranslate);
                    Task.Run(() => logicDb.Set(forTranslate, byTranslate));
                }
                else
                {
                    SetRuWord(byTranslate);
                    SetEnWord(forTranslate);
                    dictionaryWords.Add(byTranslate, forTranslate);
                    Task.Run(() => logicDb.Set(byTranslate, forTranslate));
                }
            }
        }

        public void DeleteWordInLists(int index)
        {
            DeleteInDictionary(ruList[index], enList[index]);
            int indexRu = ruAllWords.IndexOf(ruList[index]);
            int indexEn = enAllWords.IndexOf(enList[index]);
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
            if (ruList.Count != ruAllWords.Count)
            {
                ruList.RemoveAt(index);
                enList.RemoveAt(index);
            }
        }

        private void DeleteInDictionary(string ruWord, string enWord)
        {
            dictionaryWords.Remove(ruWord, enWord);
            Task.Run(() => logicDb.Delete(ruWord, enWord));
        }

        public string GetRuWord(int index)
        {
            return ruList[index];
        }

        public string GetEnWord(int index)
        {
            return enList[index];
        }

        private void SetRuWord(string word)
        {
            ruList.Add(word);
        }

        private void SetEnWord(string word)
        {
            enList.Add(word);
        }

        public int GetCount()
        {
            return ruList?.Count ?? 0;
        }

        public void SetLists((List<string>, List<string>) newLists)
        {
            ruList = newLists.Item1;
            enList = newLists.Item2;
        }

        public void SetLists()
        {
            ruList = ruAllWords;
            enList = enAllWords;
        }

        public (List<string>, List<string>) GetLists()
        {
            return (ruAllWords, enAllWords);
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

        private bool IsAlreadyExist(string firstWord, string secondWord)
        {
            List<string> listTranslate = dictionaryWords[firstWord];
            if (listTranslate != null)
            {
                return listTranslate.IndexOf(secondWord) != -1;
            }
            return false;
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
            ruList = new List<string>();
            enList = new List<string>();
            for (int i = 0; i < words.Count; i++)
            {
                await Task.Run(() => ruList.Add(words[i].RussianVariant.ToLower()));
                await Task.Run(() => enList.Add(words[i].EnglishVariant.ToLower()));
            }
            ruAllWords = ruList;
            enAllWords = enList;
        }
    }
}