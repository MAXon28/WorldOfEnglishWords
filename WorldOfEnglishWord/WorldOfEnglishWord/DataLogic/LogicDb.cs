using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite;
using WorldOfEnglishWord.Models;

namespace WorldOfEnglishWord.DataLogic
{
    class LogicDb
    {
        private readonly string databaseName;
        private readonly SQLiteConnection dataBase;

        public LogicDb()
        {
            databaseName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "WorldOfEnglishWords.db3");
            dataBase = new SQLiteConnection(databaseName);
        }

        public List<Word> GetWords()
        {
            //dataBase.DeleteAll<Word>();
            dataBase.CreateTable<Word>();
            List<Word> checkWords = dataBase.Table<Word>().Any() ? dataBase.Table<Word>().ToList() : null;
            if (checkWords == null)
            {
                List<Word> words = new List<Word>();
                words.Add(new Word()
                {
                    RussianVariant = "Привет",
                    EnglishVariant = "Hello"
                });
                words.Add(new Word()
                {
                    RussianVariant = "Мир",
                    EnglishVariant = "World"
                });
                dataBase.InsertAll(words);
                checkWords = words;
            }
            return checkWords;
        }

        public Dictionary<string, List<int>> GetRuStatistic()
        {
            Dictionary<string, List<int>> ruStatistic = new Dictionary<string, List<int>>();
            var groups = from word in dataBase.Table<Word>()
                group word by word.RussianVariant;
            int groupsIndex = 0;
            int wordsIndexInGroup = 0;
            foreach (IGrouping<string, Word> group in groups)
            {
                foreach (var word in group)
                {
                    if (wordsIndexInGroup == 0)
                    {
                        ruStatistic.Add(word.RussianVariant, new List<int>{ word.CountOfCorrectRespRusVar, word.AllCountRusVar });
                    }
                    else
                    {
                        ruStatistic[word.RussianVariant][0] += word.CountOfCorrectRespRusVar;
                        ruStatistic[word.RussianVariant][1] += word.AllCountRusVar;
                    }
                    wordsIndexInGroup++;
                }
                wordsIndexInGroup = 0;
                groupsIndex++;
            }
            return ruStatistic;
        }

        public Dictionary<string, List<int>> GetEnStatistic()
        {
            Dictionary<string, List<int>> enStatistic = new Dictionary<string, List<int>>();
            var groups = from word in dataBase.Table<Word>()
                group word by word.EnglishVariant;
            int groupsIndex = 0;
            int wordsIndexInGroup = 0;
            foreach (IGrouping<string, Word> group in groups)
            {
                foreach (var word in group)
                {
                    if (wordsIndexInGroup == 0)
                    {
                        enStatistic.Add(word.EnglishVariant, new List<int> { word.CountOfCorrectRespEnVar, word.AllCountEnVar });
                    }
                    else
                    {
                        enStatistic[word.EnglishVariant][0] += word.CountOfCorrectRespEnVar;
                        enStatistic[word.EnglishVariant][1] += word.AllCountEnVar;
                    }
                    wordsIndexInGroup++;
                }
                wordsIndexInGroup = 0;
                groupsIndex++;
            }
            return enStatistic;
        }

        public void Set(string ru, string en)
        {
            Word word = new Word();
            word.RussianVariant = ru;
            word.EnglishVariant = en;
            dataBase.Insert(word);
        }

        public void Delete(string ru, string en)
        {
            dataBase.Delete<Word>((from word in dataBase.Table<Word>()
                where word.RussianVariant == ru && word.EnglishVariant == en
                select word.Id).ToList()[0]);
        }
    }
}