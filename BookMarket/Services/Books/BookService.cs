using BookMarket.Models.DataBase;
using BookMarket.Models.ViewModels.HomeViewModels;
using BookMarket.Models.ViewModels.Profile;
using BookMarket.Models.ViewModels.SearchBook;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public async Task<List<FavoriteBook>> GetFavoritesBooks(string userName)
        {
            var favoritesBooks = await db
                .FavoriteUserBook
                .Where(i => i.UserId == userName)
                .Include("BookFavorite.IdAuthorNavigation")
                .Select(i => new FavoriteBook
                { 
                    BookName = i.BookFavorite.Name,
                    Author = i.BookFavorite.IdAuthorNavigation.NameFamily,
                    PosterBook = i.BookFavorite.PosterBook,
                    IdBook = i.IdBookFavorite,
                    Id = i.Id,
                    IdAuthor = (int)i.BookFavorite.IdAuthor,
                    DescriptionBook = i.BookFavorite.Description                    
                })
                .ToListAsync();

            return favoritesBooks;
        }

        /// <summary>
        /// Выборка топовых книг
        /// </summary>
        /// <param name="countBooks">количество книг</param>
        /// <returns>Книги</returns>
        public async Task<List<IndexBook>> GetTopBooks(int countBooks = 4)
        {
            // Получаем айдишники топовых книг
            var ids = await db.Ratings
                .GroupBy(u => u.IdBook)
                .Select(g => new
                {
                    g.Key,
                    MarkAverage = g.Average(s => s.Mark)
                })
                .OrderByDescending(i => i.MarkAverage).Take(countBooks).Select(g => g.Key).ToListAsync();

            // Получаем выборку книг
            var books = await db.Book.Where(p => ids.Contains(p.Id))
                .Select(i => new IndexBook { RatingBook = i.UserRating.Average(i => i.Mark), IdAuthor = (int)i.IdAuthor, Id = i.Id, Name = i.Name, PosterBook = i.PosterBook, AuthorNameFamily = i.IdAuthorNavigation.NameFamily })
                .ToListAsync();


            return books;

        }



        /// <summary>
        /// Выбрка последних комментариев книг
        /// </summary>
        /// <param name="CountCommentary">Количество комментариев</param>
        /// <returns>Выборка последних комментариев</returns>
        public async Task <IEnumerable<BookViewModel>> GetLastCommentaries(int CountCommentary)
        {
            // Получаем номера книг последних комментируемых
            var ids = await db.Ratings.OrderByDescending(i => i.DateCreated).Select(i => i.IdBook).Take(CountCommentary).ToListAsync();

            var books = await db.Book
                .Include("UserRating")
                .Include("IdAuthorNavigation")
                .Include("IdCategoryNavigation")
                .Where(i => ids.Contains(i.Id))
                .Select(i =>
                    new BookViewModel
                    {
                        Id = i.Id,
                        Name = i.Name,
                        AuthorNameFamily = i.IdAuthorNavigation.NameFamily,
                        CategoryName = i.IdCategoryNavigation.Name,
                        IdAuthor = (int)i.IdAuthor,
                        PosterBook = i.PosterBook,
                        RatingBook = i.UserRating.Count != 0 ? i.UserRating.Average(i => i.Mark) : 0
                    })
                .ToListAsync();

            return books;
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
                }, s => s.VisitsBook
                .OrderByDescending(i => i.Id)
                .Where(i => i.IdUser == idUser)
                .FirstOrDefault()))
                // Сортируем по дате посещения
                .OrderByDescending(i => i.Value.DateVisit).Take(CountBook != 0 ? CountBook : 1000).ToDictionary(pair => pair.Key, pair => pair.Value);



            return query;
        }


        /// <summary>
        /// Получить новые книги
        /// </summary>
        /// <param name="count">количество новых книг</param>
        /// <returns>Новые книги</returns>
        public async Task<IEnumerable<IndexBook>> GetNewsBooks(int count)
        {
            List<IndexBook> newsBook = null;

            
            newsBook = await db.Book
            .Select(i => new IndexBook { RatingBook = i.UserRating.Count != 0 ? i.UserRating.Average(i => i.Mark) : 0, IdAuthor = (int)i.IdAuthor, Id = i.Id, Name = i.Name, PosterBook = i.PosterBook, AuthorNameFamily = i.IdAuthorNavigation.NameFamily })
            .OrderByDescending(i => i.Id).Take(4).ToListAsync();


            return newsBook;
        }


        /// <summary>
        /// Выборка книг в поиске
        /// </summary>
        /// <param name="IdGenre"></param>
        /// <param name="page">страница</param>
        /// <param name="word">название книги</param>
        /// <param name="RatingOrdered">сортировка по рейтингу</param>
        /// <returns>Выборку книг</returns>
        public async Task<IEnumerable<BookViewModel>> GetSearchBooks(int IdGenre = 0, int page = 0, string word = null, byte RatingOrdered = 0)
        {
            var books = await db.Book
                .OrderByDescending(i => RatingOrdered == 1 ? i.UserRating.Average(i => i.Mark) : i.Id)
                // Фильтруем по ключевому слову
                .Where(i =>
                // по жанру
                IdGenre != 0 ? i.IdCategoryNavigation.Id == IdGenre : true
                && !string.IsNullOrEmpty(word) ? i.Name.Contains(word) || i.IdAuthorNavigation.Name.Contains(word) || i.IdAuthorNavigation.Family.Contains(word) : true)

                .Skip(page == 1 ? 0 : (page - 1) * 10)
                .Select(i =>
                new BookViewModel
                {
                    Id = i.Id,
                    AddDatabase = i.AddDatabase,
                    CategoryName = i.IdCategoryNavigation.Name,
                    Name = i.Name,
                    idCategory = IdGenre, // Поджанр книги
                    IdAuthor = (int)i.IdAuthor,
                    AuthorNameFamily = i.IdAuthorNavigation.NameFamily,
                    Description = i.Description,
                    PosterBook = i.PosterBook,
                    RatingBook = i.UserRating.Average(i => i.Mark)
                })
                .Take(10)
                .ToListAsync();


            return books;
        }


        /// <summary>
        /// Максимальное количество книг в выборке
        /// </summary>
        /// <param name="word">Название книги</param>
        /// <param name="IdGenre">Номер жанра</param>
        /// <returns>Максимальное количество книг</returns>
        public async Task<int> getCountsBooks(string word, ushort IdGenre)
        {
            var countBooks = await db.Book // Максимальное количество книг в выборке
                                .Where(i =>
                                // по жанру
                                IdGenre != 0 ? i.IdCategoryNavigation.Id == IdGenre : true
                                && !string.IsNullOrEmpty(word) ? i.Name.Contains(word) || i.IdAuthorNavigation.Name.Contains(word) || i.IdAuthorNavigation.Family.Contains(word) : true)
                                .CountAsync();

            return countBooks;
        }
    }
}
