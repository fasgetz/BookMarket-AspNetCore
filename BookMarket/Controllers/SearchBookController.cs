using BookMarket.Models.DataBase;
using BookMarket.Models.ViewModels.SearchBook;
using BookMarket.Services.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Controllers
{
    public class SearchBookController : Controller
    {
        IBookService bookService;
        BookMarketContext db;

        public SearchBookController(BookMarketContext _db, IBookService bookService)
        {
            this.bookService = bookService;
            this.db = _db;
        }


        [HttpGet]
        public async Task<IActionResult> getData(int IdGenre, int page, string word, byte RatingOrdered = 0)
        {
            var Books = await db.Book                
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
                    IdAuthor = (int)i.IdAuthor,
                    AuthorNameFamily = i.IdAuthorNavigation.NameFamily,
                    Description = i.Description,
                    PosterBook = i.PosterBook,
                    RatingBook = i.UserRating.Average(i => i.Mark)
                })                
                .Take(10)
                .ToListAsync();

            // Выбираем первые 50 слов
            foreach (var item in Books)
                item.Description = string.Join(' ', item.Description.Split(' ').Take(50));


            getDataViewModel vm = new getDataViewModel()
                {
                    RatingOrdered = RatingOrdered == 1 ? true : false,
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
        public async Task<IActionResult> LastVisitBooks()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Запрос на книги
                var query = await bookService.GetLastVisitBook(User.Identity.Name, 5);

                return PartialView(query);
            }
            



            

            return null;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string word, int idGenre, bool RatingOrdered = false)
        {
            SearchBookIndexVM vm = new SearchBookIndexVM()
            {
                RatingOrdered = RatingOrdered,
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
