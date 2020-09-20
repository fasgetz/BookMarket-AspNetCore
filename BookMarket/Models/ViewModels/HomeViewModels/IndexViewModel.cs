using BookMarket.Models.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.ViewModels.HomeViewModels
{
    public class IndexViewModel
    {
        /// <summary>
        /// Новые книги
        /// </summary>
        public List<Book> newsBooks { get; set; }

        /// <summary>
        /// Для отображения категории жанров
        /// </summary>
        public IDictionary<CategoryGenre, System.Collections.Generic.List<GenreBook>> CategoryGenres { get; set; }
    }
}
