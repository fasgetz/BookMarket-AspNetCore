using BookMarket.Models.DataBase;
using BookMarket.Models.ViewModels.SearchBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.ViewModels.HomeViewModels
{
    public class IndexViewModel
    {

        /// <summary>
        /// Топовые книги
        /// </summary>
        public List<IndexBook> topBooks { get; set; }

        /// <summary>
        /// Новые книги
        /// </summary>
        public List<IndexBook> newsBooks { get; set; }

        /// <summary>
        /// Для отображения категории жанров
        /// </summary>
        public IDictionary<CategoryGenre, System.Collections.Generic.List<GenreBook>> CategoryGenres { get; set; }

        /// <summary>
        /// Последние комментарии книг
        /// </summary>
        public IDictionary<BookViewModel, Rating> lastCommentBooks { get; set; }

        /// <summary>
        /// Рейтинг пользователей
        /// </summary>
        public ICollection<TopUser> topUsers { get; set; }


        /// <summary>
        /// Новые пользователи
        /// </summary>
        public ICollection<TopUser> NewUsers { get; set; }
    }
}
