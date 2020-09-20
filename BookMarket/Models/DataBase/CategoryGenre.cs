using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.DataBase
{
    /// <summary>
    /// Категория жанра
    /// </summary>
    public class CategoryGenre
    {
        public CategoryGenre()
        {
            GenresBook = new HashSet<GenreBook>();
        }



        public int Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Жанры книги
        /// </summary>
        public virtual ICollection<GenreBook> GenresBook { get; set; }
    }
}
