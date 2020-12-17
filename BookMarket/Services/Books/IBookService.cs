﻿using BookMarket.Models.DataBase;
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
        public Task<IEnumerable<KeyValuePair<BookViewModel, Rating>>> GetLastCommentaries(int CountCommentary);

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
        Task<List<IndexBook>> GetNewsBooks(int count);
    }
}
