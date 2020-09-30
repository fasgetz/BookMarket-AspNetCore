using BookMarket.Models.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.ViewModels.SearchBook
{
    public class SearchBookIndexVM
    {
        public bool RatingOrdered { get; set; } // Сортировка по рейтингу

        /// <summary>
        /// Строка ввода в поиске
        /// </summary>
        public string WordInput { get; set; }
        /// <summary>
        /// Айди жанра
        /// </summary>
        public int IdGenre { get; set; }

        /// <summary>
        /// Для отображения категории жанров
        /// </summary>
        public IDictionary<CategoryGenreVM, System.Collections.Generic.List<GenreBookVM>> CategoryGenres { get; set; }

    }
}
