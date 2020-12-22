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
using BookMarket.Services.Genres;
using Microsoft.Extensions.DependencyInjection;

namespace BookMarket.Controllers
{



    public class HomeController : Controller
    {

        public IBookService serviceBooks { get; private set; }
        public IUserService userService { get; private set; }
        public IGenresService genresService { get; private set; }



        #region Тесты

        public HomeController(IBookService serviceBooks)
        {
            this.serviceBooks = serviceBooks;
        }


        public HomeController(IUserService userService, IGenresService genresService)
        {
            this.userService = userService;
            this.genresService = genresService;
        }

        #endregion


        [ActivatorUtilitiesConstructor]
        public HomeController(IGenresService genresService, IBookService serviceBooks, IUserService userService)
        {
            this.userService = userService;
            this.serviceBooks = serviceBooks;
            this.genresService = genresService;
        }


        /// <summary>
        /// Выборка последних комментируемых книг
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> LastCommentsBook(int count = 4)
        {
            var lastCommentBooks = await serviceBooks.GetLastCommentaries(count);

            return PartialView("newComments", lastCommentBooks);
        }

        /// <summary>
        /// Выборка недавно добавленных книг
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> newBooks(int count = 4)
        {
            var newBooks = await serviceBooks.GetNewsBooks(count);

            return PartialView("booksData", newBooks);
        }

        /// <summary>
        /// Выборка топовых книг
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> topBooks(int count = 4)
        {

            // Получаем айдишники топовых книг

            var books = await serviceBooks.GetTopBooks(count);
            


            return PartialView("topBooksData", books);

        }


        [HttpGet]
        public async Task<IActionResult> IndexData(int genresCount = 8, int subGenresCount = 3, int topUsers = 10, int newUsers = 10)
        {

            IndexViewModel vm = new IndexViewModel()
            {

                CategoryGenres = await genresService.GetGenresSubCategories(genresCount, subGenresCount),

                topUsers = await userService.GetTopUsers(topUsers),
                NewUsers = await userService.GetNewUsers(newUsers)
            };



            return PartialView(vm);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
