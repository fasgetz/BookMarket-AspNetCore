using BookMarket.Models.DataBase;
using BookMarket.Models.ViewModels.HomeViewModels;
using BookMarket.Models.ViewModels.Profile;
using BookMarket.Models.ViewModels.SearchBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Services.Books
{
    public interface IBookService
    {

        /// <summary>
        /// Выборка книг в поиске
        /// </summary>
        /// <param name="IdGenre"></param>
        /// <param name="page">страница</param>
        /// <param name="word">название книги</param>
        /// <param name="RatingOrdered">сортировка по рейтингу</param>
        /// <returns>Выборку книг</returns>

        public Task<IEnumerable<BookViewModel>> GetSearchBooks(int IdGenre = 0, int page = 0, string word = null, byte RatingOrdered = 0);

        /// <summary>
        /// Выборка топовых книг
        /// </summary>
        /// <param name="countBooks">количество книг</param>
        /// <returns>Книги</returns>
        public Task<List<IndexBook>> GetTopBooks(int countBooks = 4);



        Task<List<FavoriteBook>> GetFavoritesBooks(string userName);

        /// <summary>
        /// Выборка последних посещенных книг пользователя
        /// </summary>
        /// <param name="idUser">Айди пользователя</param>
        /// <param name="CountBook">Количество книг</param>
        /// <returns>Последние посещенные книги</returns>
        Task<IDictionary<BookViewModel, visitUser>> GetLastVisitBook(string idUser, int CountBook = 0);

        /// <summary>
        /// Выборка последних комментариев книг
        /// </summary>
        /// <param name="CountCommentary">Количество комментариев</param>
        /// <returns>Выборка последних комментариев</returns>
        public Task<IEnumerable<BookViewModel>> GetLastCommentaries(int CountCommentary);

        /// <summary>
        /// Выборка последних комментариев юзера
        /// </summary>
        /// <param name="name">Имя юзера</param>
        /// <returns>Выборка комментариев юзера</returns>
        Task<IList<Rating>> GetLastCommentariesUser(string name);


        /// <summary>
        /// Получить новые книги
        /// </summary>
        /// <param name="count">количество новых книг</param>
        /// <returns>Новые книги</returns>
        Task<IEnumerable<IndexBook>> GetNewsBooks(int count);


        /// <summary>
        /// Максимальное количество книг в выборке
        /// </summary>
        /// <param name="word">Название книги</param>
        /// <param name="IdGenre">Номер жанра</param>
        /// <returns>Максимальное количество книг</returns>
        Task<int> getCountsBooks(string word, ushort IdGenre);
    }
}
