using BookMarket.Models.DataBase;
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
        /// Выборка последних посещенных книг пользователя
        /// </summary>
        /// <param name="idUser">Айди пользователя</param>
        /// <param name="CountBook">Количество книг</param>
        /// <returns>Последние посещенные книги</returns>
        Task<IDictionary<BookViewModel, visitUser>> GetLastVisitBook(string idUser, int CountBook);

        /// <summary>
        /// Выбрка последних комментариев книг
        /// </summary>
        /// <param name="CountCommentary">Количество комментариев</param>
        /// <returns>Выборка последних комментариев</returns>
        Task<IDictionary<BookViewModel, Rating>> GetLastCommentaries(int CountCommentary);
    }
}
