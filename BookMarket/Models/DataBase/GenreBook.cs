using System;
using System.Collections.Generic;

namespace BookMarket.Models.DataBase
{

    /// <summary>
    /// Жанр книги
    /// </summary>
    public class GenreBook
    {


        public GenreBook()
        {
            Book = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string Name { get; set; }


        /// <summary>
        /// Ссылка на категорию жанра
        /// </summary>
        public int? IdGenreCategory { get; set; }
        public virtual CategoryGenre GenreCategory { get; set; }

        public virtual ICollection<Book> Book { get; set; }
    }
}
