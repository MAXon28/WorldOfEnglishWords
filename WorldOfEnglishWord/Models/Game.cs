using System;
using SQLite;

namespace WorldOfEnglishWord.Models
{
    public class Game
    {
        [PrimaryKey, AutoIncrement, Unique]
        public Guid Id { set; get; }

        [NotNull]
        public int AllGames { get; set; }

        [NotNull]
        public int NumberOfPoints { get; set; }
    }
}