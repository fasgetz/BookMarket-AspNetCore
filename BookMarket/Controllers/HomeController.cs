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
using BookMarket.Services;
using BookMarket.Models.UsersIdentity;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace BookMarket.Controllers
{

    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize<T>(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
        }
    }



    public class HomeController : Controller
    {

        private readonly IBookService serviceBooks;
        private readonly IUserService userService;
        private readonly ILogger<HomeController> _logger;
        BookMarketContext context;

        public HomeController(ILogger<HomeController> logger, BookMarketContext context, IBookService serviceBooks, IUserService userService)
        {
            this.userService = userService;
            _logger = logger;
            this.serviceBooks = serviceBooks;
            this.context = context;
        }


        /// <summary>
        /// Выборка последних комментируемых книг
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> LastCommentsBook()
        {
            var lastCommentBooks = await serviceBooks.GetLastCommentaries(4);

            return PartialView("newComments", lastCommentBooks);
        }

        /// <summary>
        /// Выборка недавно добавленных книг
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> newBooks()
        {
            var newBooks = await serviceBooks.GetNewsBooks(4);

            return PartialView("booksData", newBooks);
        }

        /// <summary>
        /// Выборка топовых книг
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> topBooks()
        {
            var topBooks = await context.Book
                .Select(i => new IndexBook { RatingBook = i.UserRating.Count != 0 ? i.UserRating.Average(i => i.Mark) : 0, IdAuthor = (int)i.IdAuthor, Id = i.Id, Name = i.Name, PosterBook = i.PosterBook, AuthorNameFamily = i.IdAuthorNavigation.NameFamily })
                .OrderByDescending(i => i.RatingBook).Take(4).ToListAsync();

            return PartialView("topBooksData", topBooks);
        }


        [HttpGet]
        public async Task<IActionResult> IndexData()
        {

            IndexViewModel vm = new IndexViewModel()
            {

                CategoryGenres = await context.GenreCategory
                                    .Include("GenresBook")
                                    .Take(8)
                                    .ToDictionaryAsync(i => new CategoryGenre() { Id = i.Id, Name = i.Name }, s => s.GenresBook.Take(3).ToList()),
 
                topUsers = await userService.GetTopUsers(10),
                NewUsers = await userService.GetNewUsers(10)
            };



            return PartialView(vm);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        public FileResult GetFileFromBytes(byte[] bytesIn)
        {
            return File(bytesIn, "image/png");
        }

        [HttpGet]
        public IActionResult GetImage(int idBook)
        {
            var book = context.Book.FirstOrDefault(i => i.Id == idBook).PosterBook;
            FileResult image = GetFileFromBytes(book);
            return image;
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
