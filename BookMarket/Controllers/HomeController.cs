using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookMarket.Models;
using BookMarket.Models.DataBase;
using Microsoft.EntityFrameworkCore;
using System;
using BookMarket.Models.ViewModels.HomeViewModels;
using System.Threading.Tasks;
using BookMarket.Models.ViewModels.SearchBook;
using BookMarket.Services.Books;

namespace BookMarket.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService serviceBooks;
        private readonly ILogger<HomeController> _logger;
        BookMarketContext context;

        public HomeController(ILogger<HomeController> logger, BookMarketContext context, IBookService serviceBooks)
        {
            _logger = logger;
            this.serviceBooks = serviceBooks;
            this.context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IndexViewModel vm = new IndexViewModel()
            {
                topBooks = await context.Book
                .Select(i => new IndexBook { RatingBook = i.UserRating.Count != 0 ? i.UserRating.Average(i => i.Mark) : 0, IdAuthor = (int)i.IdAuthor, Id = i.Id, Name = i.Name, PosterBook = i.PosterBook, AuthorNameFamily = i.IdAuthorNavigation.NameFamily })
                .OrderByDescending(i => i.RatingBook).Take(4).ToListAsync(),
                newsBooks = await context.Book
                .Select(i => new IndexBook { RatingBook = i.UserRating.Count != 0 ? i.UserRating.Average(i => i.Mark) : 0, IdAuthor = (int)i.IdAuthor, Id = i.Id, Name = i.Name, PosterBook = i.PosterBook, AuthorNameFamily = i.IdAuthorNavigation.NameFamily })
                .OrderByDescending(i => i.Id).Take(4).ToListAsync(),
                CategoryGenres = await context.GenreCategory
                .Include("GenresBook")
                .Take(8)
                .ToDictionaryAsync(i => new CategoryGenre() { Id = i.Id, Name = i.Name }, s => s.GenresBook.Take(3).ToList()),
                lastCommentBooks = await serviceBooks.GetLastCommentaries(4)
            };
   

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
