using BookMarket.Models.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Services.Books
{
    public interface IBookService
    {
        ICollection<Book> GetLastVisitBook(string idUser, int CountBook);
    }
}
