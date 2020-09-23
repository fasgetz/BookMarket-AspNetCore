using BookMarket.Models.DataBase;
using BookMarket.Models.ViewModels.SearchBook;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Controllers
{
    public class SearchBookController : Controller
    {
        BookMarketContext db;

        public SearchBookController(BookMarketContext _db)
        {
            this.db = _db;
        }


        [HttpGet]
        public IActionResult getData(string word, int page)
        {
            SearchBookIndexVM vm = new SearchBookIndexVM()
            {               
                Books = db.Book
                    .Include("IdAuthorNavigation")
                    .Include("IdCategoryNavigation")
                    .OrderByDescending(i => i.Id)
                    // Фильтруем по ключевому слову
                    .Where(i => !string.IsNullOrEmpty(word) ? i.Name.Contains(word) || i.IdAuthorNavigation.Name.Contains(word) || i.IdAuthorNavigation.Family.Contains(word) : true)
                    .Take(10)
                    .ToList()
            };



            return PartialView("SearchedBooks", vm);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string word)
        {
            SearchBookIndexVM vm = new SearchBookIndexVM()
            {
                // Выборка категорий жанров
                CategoryGenres = await db.GenreCategory
                    .Include("GenresBook")
                    .ToDictionaryAsync(i => new CategoryGenreVM() { Id = i.Id, Name = i.Name }, s => s.GenresBook.Select(r => new GenreBookVM { Id = r.Id, Name = r.Name, CountBooks = db.Book.Count(i => i.IdCategory == r.Id) }).ToList())                
            };

            return View(vm);
        }
    }
}
