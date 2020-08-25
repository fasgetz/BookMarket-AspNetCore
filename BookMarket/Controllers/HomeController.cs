using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookMarket.Models;
using BookMarket.Models.DataBase;

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

        public IActionResult Index()
        {

            EFGenericRepositoryPattern.EFGenericRepository<CategoryBook> rep = new EFGenericRepositoryPattern.EFGenericRepository<CategoryBook>(context);

            return View(rep.GetQueryList(i => i.Id < 3));
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
