using System;
using SQLite;

namespace WorldOfEnglishWord.Models
{
    class Word
    {
        [PrimaryKey, AutoIncrement, Unique]
        public Guid Id { set; get; }

        [NotNull]
        public string RussianVariant { get; set; }

        [NotNull]
        public string EnglishVariant { get; set; }

        public int AllCountRusVar { get; set; }

        public int CountOfCorrectRespRusVar { get; set; }

        public int AllCountEnVar { get; set; }

        public int CountOfCorrectRespEnVar { get; set; }
    }
}