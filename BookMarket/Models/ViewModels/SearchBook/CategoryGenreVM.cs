using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.ViewModels.SearchBook
{
    public class CategoryGenreVM
    {
        public int Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Жанры книги
        /// </summary>
        public virtual ICollection<GenreBookVM> GenresBook { get; set; }
    }
}
