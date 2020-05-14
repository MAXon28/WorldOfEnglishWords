using System;
using System.Collections.Generic;
using System.Linq;

namespace MyDictionary
{
    public class DictionaryKeyKey
    {
        private Dictionary<string, List<string>> dictionaryFirstVar;
        private Dictionary<string, List<string>> dictionarySecondVar;
        private int count;

        public DictionaryKeyKey()
        {
            dictionaryFirstVar = new Dictionary<string, List<string>>();
            dictionarySecondVar = new Dictionary<string, List<string>>();
        }

        public void Add(string keyFirst, string keySecond)
        {
            try
            {
                dictionaryFirstVar[keyFirst].Add(keySecond);
            }
            catch(Exception)
            {
                dictionaryFirstVar.Add(keyFirst, new List<string>() { keySecond });
            }

            try
            {
                dictionarySecondVar[keySecond].Add(keyFirst);
            }
            catch(Exception)
            {
                dictionarySecondVar.Add(keySecond, new List<string> { keyFirst });
            }

            count++;
        }

        public void Remove(string firstVar, string secondVar)
        {
            dictionaryFirstVar[firstVar].Remove(secondVar);
            if(dictionaryFirstVar[firstVar].Count == 0)
            {
                dictionaryFirstVar.Remove(firstVar);
            }

            dictionarySecondVar[secondVar].Remove(firstVar);
            if(dictionarySecondVar[secondVar].Count == 0)
            {
                dictionarySecondVar.Remove(secondVar);
            }
        }

        public List<string> this[string key]
        {
            get
            {
                List<string> result = new List<string>();
                result = GetFirstList(key);
                if (result == null)
                {
                    result = GetSecondList(key);
                }
                return result;
            }
        }

        public static List<string> GetKeys(DictionaryKeyKey dictionary)
        {
            return dictionary.dictionaryFirstVar.Keys.ToList();
        }

        public static int Count(DictionaryKeyKey dictionary)
        {
            return dictionary.count;
        }

        private List<string> GetFirstList(string key)
        {
            try
            {
                return dictionaryFirstVar[key];
            }
            catch(Exception)
            {
                return null;
            }
        }

        private List<string> GetSecondList(string key)
        {
            try
            {
                return dictionarySecondVar[key];
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}