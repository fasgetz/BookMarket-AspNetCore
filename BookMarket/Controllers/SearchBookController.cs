using BookMarket.Models.DataBase;
using BookMarket.Models.ViewModels.SearchBook;
using BookMarket.Services.Books;
using BookMarket.Services.Genres;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Controllers
{
    public class SearchBookController : Controller
    {
        IBookService bookService;
        IGenresService genresService;

        public SearchBookController(IGenresService genresService, IBookService bookService)
        {
            this.bookService = bookService;
            this.genresService = genresService;
        }


        [HttpGet]
        public async Task<IActionResult> getData(int IdGenre = 0, int page = 0, string word = null, byte RatingOrdered = 0)
        {
            var books = await bookService.GetSearchBooks(IdGenre, page, word, RatingOrdered);

            // Выбираем первые 50 слов
            foreach (var item in books)
                item.Description = item.Description != null ? string.Join(' ', item.Description.Split(' ').Take(50)) : null;

            var genre = await genresService.FindGenreBook(IdGenre);

            getDataViewModel vm = new getDataViewModel()
                {
                    RatingOrdered = RatingOrdered == 1 ? true : false,
                    IdGenre = IdGenre,
                    InputWord = word,
                    page = page, // Текущая страница
                    MaxCountBooks = await bookService.getCountsBooks(word, (ushort)IdGenre), // Максимальное количество книг в выборке
                    books = books,
                    GenreName = genre?.Name
                };



            return PartialView("SearchedBooks", vm);
        }

        [HttpGet]
        public async Task<IActionResult> LastVisitBooks(string name)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Запрос на книги
                var query = await bookService.GetLastVisitBook(name, 5);

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
                // Выборка жанров с поджанрами с количеством книг
                CategoryGenres = await genresService.getGenresSubCategoriesCounts()
            };

            return View(vm);
        }
    }
}
