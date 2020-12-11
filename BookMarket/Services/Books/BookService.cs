using BookMarket.Models.DataBase;
using BookMarket.Models.ViewModels.HomeViewModels;
using BookMarket.Models.ViewModels.Profile;
using BookMarket.Models.ViewModels.SearchBook;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Services.Books
{
    public class BookService : IBookService
    {
        BookMarketContext db;
        private IMemoryCache cache;

        public BookService(BookMarketContext db, IMemoryCache memoryCache)
        {
            this.db = db;
            cache = memoryCache;
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
        /// Выбрка последних комментариев книг
        /// </summary>
        /// <param name="CountCommentary">Количество комментариев</param>
        /// <returns>Выборка последних комментариев</returns>
        public async Task<IDictionary<BookViewModel, Rating>> GetLastCommentaries(int CountCommentary)
        {
            Dictionary<BookViewModel, Rating> lastComments = null;

            lastComments = (await db.Book
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
                s => s.UserRating.OrderByDescending(i => i.Id).FirstOrDefault()));

            //var haveCashed = cache.TryGetValue("lastCommentsBook", out lastComments);

            //// Если есть в кеше, то выгрузи
            //if (haveCashed == false)
            //{
            //    // Иначе загрузить
            //    lastComments = (await db.Book
            //    .Where(i => i.UserRating.Count() != 0)
            //    .Include("UserRating")
            //    .Include("IdAuthorNavigation")
            //    .Include("IdCategoryNavigation")
            //    .ToDictionaryAsync(i =>
            //    new BookViewModel
            //    {
            //        Id = i.Id,
            //        Name = i.Name,
            //        AuthorNameFamily = i.IdAuthorNavigation.NameFamily,
            //        CategoryName = i.IdCategoryNavigation.Name,
            //        IdAuthor = (int)i.IdAuthor,
            //        PosterBook = i.PosterBook,
            //        RatingBook = i.UserRating.Count != 0 ? i.UserRating.Average(i => i.Mark) : 0
            //    },
            //    s => s.UserRating.OrderByDescending(i => i.Id).FirstOrDefault()))
            //    // Сортируем по дате посещения

            //    .OrderByDescending(i => i.Value.Id).Take(4).ToDictionary(pair => pair.Key, pair => pair.Value);

            //    // Записать в кеш
            //    if (lastComments != null)
            //    {
            //        cache.Set("lastCommentsBook", lastComments,
            //        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1 * 60 * 24)));
            //    }
            //}


            //// Выборка последних 4-х комментариев книгам
            //var lastCommentsBooks = (await db.Book
            //    .Where(i => i.UserRating.Count() != 0)
            //    .Include("UserRating")
            //    .Include("IdAuthorNavigation")
            //    .Include("IdCategoryNavigation")
            //    .ToDictionaryAsync(i =>
            //    new BookViewModel
            //    {
            //        Id = i.Id,
            //        Name = i.Name,
            //        AuthorNameFamily = i.IdAuthorNavigation.NameFamily,
            //        CategoryName = i.IdCategoryNavigation.Name,
            //        IdAuthor = (int)i.IdAuthor,
            //        PosterBook = i.PosterBook,
            //        RatingBook = i.UserRating.Count != 0 ? i.UserRating.Average(i => i.Mark) : 0
            //    },
            //    s => s.UserRating.OrderByDescending(i => i.Id).FirstOrDefault()))
            //    // Сортируем по дате посещения

            //    .OrderByDescending(i => i.Value.Id).Take(4).ToDictionary(pair => pair.Key, pair => pair.Value);

            return lastComments;
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
        public async Task<List<IndexBook>> GetNewsBooks(int count)
        {
            List<IndexBook> newsBook = null;

            
            newsBook = await db.Book
            .Select(i => new IndexBook { RatingBook = i.UserRating.Count != 0 ? i.UserRating.Average(i => i.Mark) : 0, IdAuthor = (int)i.IdAuthor, Id = i.Id, Name = i.Name, PosterBook = i.PosterBook, AuthorNameFamily = i.IdAuthorNavigation.NameFamily })
            .OrderByDescending(i => i.Id).Take(4).ToListAsync();


            //// Иначе загрузить
            //var haveCashed = cache.TryGetValue("newsBook", out newsBook);
            //// Если есть в кеше, то выгрузи
            //if (haveCashed == false)
            //{
            //    // Иначе загрузить
            //    newsBook = await db.Book
            //    .Select(i => new IndexBook { RatingBook = i.UserRating.Count != 0 ? i.UserRating.Average(i => i.Mark) : 0, IdAuthor = (int)i.IdAuthor, Id = i.Id, Name = i.Name, PosterBook = i.PosterBook, AuthorNameFamily = i.IdAuthorNavigation.NameFamily })
            //    .OrderByDescending(i => i.Id).Take(4).ToListAsync();

            //    // Записать в кеш
            //    if (newsBook != null)
            //    {
            //        cache.Set("newsBook", newsBook,
            //        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1*60*24)));
            //    }
            //}


            return newsBook;
        }
    }
}
