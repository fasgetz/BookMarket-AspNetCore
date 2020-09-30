using BookMarket.Models.DataBase;
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

        public ICollection<Book> GetLastVisitBook(string idUser, int CountBook)
        {
            throw new NotImplementedException();
        }
    }
}
