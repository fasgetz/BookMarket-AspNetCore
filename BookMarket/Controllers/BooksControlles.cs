using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookMarket.Models.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace BookMarket.Controllers
{
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        BookMarketContext context;
        public BooksController(ILogger<BooksController> logger, BookMarketContext context)
        {
            _logger = logger;

            this.context = context;
        }

        // GET: Books 
        public async Task<IActionResult> Index()
        {
            return View(await Task.Run(() => context.Book.ToList()));
        }

        // GET: Books/About/5
        public async Task<IActionResult> AboutBook(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await context.Book.FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View("About", book);
        }
    }
}
