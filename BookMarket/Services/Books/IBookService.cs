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
        Task<IDictionary<BookViewModel, visitUser>> GetLastVisitBook(string idUser, int CountBook);
    }
}
