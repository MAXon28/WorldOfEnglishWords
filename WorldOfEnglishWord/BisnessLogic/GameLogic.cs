using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldOfEnglishWord.Models;

namespace WorldOfEnglishWord.BisnessLogic
{
    public class GameLogic
    {
        private LogicDb logicDb;

        public GameLogic()
        {
            logicDb = WordsLogic.GetInstance().GetLogicDb();
        }

        public void InstallData()
        {
            Game = logicDb.GetDataGame();
        }

        public Game Game { get; set; }

        public (string[], string[]) GetWordsForGame()
        {
            string[] fullWords = new string[5];
            string[] clippedWords = new string[5];
            List<string> enList = WordsLogic.GetInstance().GetEnAllWords();
            int index = 0;
            int maxWords = enList.Count >= 5 ? 5 : enList.Count;
            while (index != maxWords)
            {
                string word = enList[new Random().Next(0, enList.Count)];
                if (word.Length > 1 && !fullWords.Contains(word))
                {
                    fullWords[index] = word;
                    clippedWords[index] = TrimWord(fullWords[index]);
                    index++;
                }
            }
            return (fullWords, clippedWords);
        }

        private string TrimWord(string word)
        {
            int length = word.Length;
            int countDeleteLetters = new Random().Next(1, 5);
            if (length < 4 || countDeleteLetters == 1)
            {
                return word.Replace(word[new Random().Next(0, length)], '_');
            }
            else if (length < 7 || countDeleteLetters == 2)
            {
                return ApplyTrimAlgorithm(word, 2);
            }
            else if (length < 10 || countDeleteLetters == 3)
            {
                return ApplyTrimAlgorithm(word, 3);
            }
            else
            {
                return ApplyTrimAlgorithm(word, 4);
            }
        }

        private string ApplyTrimAlgorithm(string word, int countReplace)
        {
            string trimWord = "";
            int indexForReplace = new Random().Next(0, word.Length);
            for (int i = 0; i < countReplace; i++)
            {
                if (i == 0)
                {
                    trimWord = word.Replace(word[indexForReplace], '_');
                    if (indexForReplace + 2 >= word.Length)
                    {
                        indexForReplace -= 2;
                    }
                    else
                    {
                        indexForReplace += 2;
                    }
                }
                else
                {
                    trimWord = trimWord.Replace(trimWord[indexForReplace], '_');
                    if (countReplace > 2)
                    {
                        while (trimWord[indexForReplace] == '_')
                        {
                            indexForReplace = new Random().Next(0, trimWord.Length);
                        }
                    }
                }
            }
            return trimWord;
        }

        public async void UpdateDataGameAsync()
        {
            await Task.Run(() => logicDb.UpdateGame(Game));
        }
    }
}