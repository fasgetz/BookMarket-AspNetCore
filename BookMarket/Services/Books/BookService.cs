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
        /// Вернуть последние просмотренные книги
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="CountBook"></param>
        /// <returns></returns>
        public async Task<IDictionary<BookViewModel, visitUser>> GetLastVisitBook(string idUser, int CountBook)
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
                .OrderByDescending(i => i.Value.DateVisit).Take(CountBook).ToDictionary(pair => pair.Key, pair => pair.Value);



            return query;
        }
    }
}
