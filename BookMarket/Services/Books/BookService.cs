using BookMarket.Models.DataBase;
using BookMarket.Models.ViewModels.SearchBook;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Services.Books
{
    public class BookService : IBookService
    {
        BookMarketContext db;

        public BookService(BookMarketContext db)
        {
            this.db = db;
        }


        /// <summary>
        /// Выбрка последних комментариев книг
        /// </summary>
        /// <param name="CountCommentary">Количество комментариев</param>
        /// <returns>Выборка последних комментариев</returns>
        public async Task<IDictionary<BookViewModel, Rating>> GetLastCommentaries(int CountCommentary)
        {
            // Выборка последних 4-х комментариев книгам
            var lastCommentsBooks = (await db.Book
                .Where(i => i.UserRating.Count() != 0)
                .Include("UserRating")
                .Include("IdAuthorNavigation")
                .Include("IdCategoryNavigation")
                .ToDictionaryAsync(i =>
                new BookViewModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    AuthorNameFamily = i.IdAuthorNavigation.NameFamily,
                    CategoryName = i.IdCategoryNavigation.Name,
                    IdAuthor = (int)i.IdAuthor,
                    PosterBook = i.PosterBook,
                    RatingBook = i.UserRating.Count != 0 ? i.UserRating.Average(i => i.Mark) : 0
                },
                s => s.UserRating.OrderByDescending(i => i.Id).FirstOrDefault()))
                // Сортируем по дате посещения

                .OrderByDescending(i => i.Value.Id).Take(4).ToDictionary(pair => pair.Key, pair => pair.Value);

            return lastCommentsBooks;
        }

        /// <summary>
        /// Выборка последних комментариев юзера
        /// </summary>
        /// <param name="name">Имя юзера</param>
        /// <returns>Выборка комментариев юзера</returns>
        public async Task<IList<Rating>> GetLastCommentariesUser(string name)
        {
            var query = await db.Ratings
                            .Where(i => i.IdUser == name)
                            .OrderByDescending(i => i.DateCreated)
                           .Include("BookRating")                           
                           .ToListAsync();

            return query;
        }


        /// <summary>
        /// Выборка последних посещенных книг пользователя
        /// </summary>
        /// <param name="idUser">Айди пользователя</param>
        /// <param name="CountBook">Количество книг</param>
        /// <returns>Последние посещенные книги</returns>
        public async Task<IDictionary<BookViewModel, visitUser>> GetLastVisitBook(string idUser, int CountBook = 0)
        {
            var query = (await db.Book
                .Include("VisitsBook")
                .Include("IdAuthorNavigation")
                .Include("IdCategoryNavigation")
                .Where(i => i.VisitsBook.Where(i => i.IdUser == idUser).Count() != 0)
                .ToDictionaryAsync(i =>
                new BookViewModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    AuthorNameFamily = i.IdAuthorNavigation.NameFamily,
                    CategoryName = i.IdCategoryNavigation.Name,
                    IdAuthor = (int)i.IdAuthor,
                    PosterBook = i.PosterBook,
                }, s => s.VisitsBook.OrderByDescending(i => i.Id).FirstOrDefault()))
                // Сортируем по дате посещения
                .OrderByDescending(i => i.Value.DateVisit).Take(CountBook != 0 ? CountBook : 1000).ToDictionary(pair => pair.Key, pair => pair.Value);



            return query;
        }
    }
}
