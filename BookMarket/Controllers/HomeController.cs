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

namespace BookMarket.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        BookMarketContext context;

        public HomeController(ILogger<HomeController> logger, BookMarketContext context)
        {
            _logger = logger;

            this.context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using (context = new BookMarketContext())
            {


                IndexViewModel vm = new IndexViewModel()
                {
                    newsBooks = await context.Book.Include("IdAuthorNavigation").OrderByDescending(i => i.Id).Take(4).ToListAsync(),
                    CategoryGenres = await context.GenreCategory
                    .Include("GenresBook")
                    .Take(8)
                    .ToDictionaryAsync(i => new CategoryGenre() { Id = i.Id, Name = i.Name }, s => s.GenresBook.Take(3).ToList())
                };

                return View(vm);

            }
                
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
