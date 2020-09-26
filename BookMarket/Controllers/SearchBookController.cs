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
        public async Task<IActionResult> getData(int IdGenre, int page, string word)
        {
            var Books = await db.Book
                .Include("IdAuthorNavigation")
                .Include("IdCategoryNavigation")
                .OrderByDescending(i => i.Id)
                // Фильтруем по ключевому слову
                .Where(i =>
                // по жанру
                IdGenre != 0 ? i.IdCategoryNavigation.Id == IdGenre : true
                && !string.IsNullOrEmpty(word) ? i.Name.Contains(word) || i.IdAuthorNavigation.Name.Contains(word) || i.IdAuthorNavigation.Family.Contains(word) : true)
                .Skip(page == 1 ? 0 : (page - 1) * 10)
                .Take(10)
                .ToListAsync();


                getDataViewModel vm = new getDataViewModel()
                {
                    IdGenre = IdGenre,
                    InputWord = word,
                    page = page, // Текущая страница
                    MaxCountBooks = db.Book // Максимальное количество книг в выборке
                    .Where(i =>
                    // по жанру
                    IdGenre != 0 ? i.IdCategoryNavigation.Id == IdGenre : true
                    && !string.IsNullOrEmpty(word) ? i.Name.Contains(word) || i.IdAuthorNavigation.Name.Contains(word) || i.IdAuthorNavigation.Family.Contains(word) : true)
                    .Count(),
                    books = Books,
                    GenreName = IdGenre != 0 ? db.GenreBooks.FirstOrDefault(i => i.Id == IdGenre).Name : null
                };



            return PartialView("SearchedBooks", vm);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string word, int idGenre)
        {
            SearchBookIndexVM vm = new SearchBookIndexVM()
            {
                IdGenre = idGenre,
                WordInput = word,
                // Выборка категорий жанров
                CategoryGenres = await db.GenreCategory
                    .Include("GenresBook")
                    .ToDictionaryAsync(i => new CategoryGenreVM() { Id = i.Id, Name = i.Name }, s => s.GenresBook.Select(r => new GenreBookVM { Id = r.Id, Name = r.Name, CountBooks = db.Book.Count(i => i.IdCategory == r.Id) }).ToList())                
            };

            return View(vm);
        }
    }
}
