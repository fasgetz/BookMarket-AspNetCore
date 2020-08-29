using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookMarket.Models.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace BookMarket.Controllers
{
    //[Route("book")]
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
        //[Route("AboutBook/{id}")]
        public async Task<IActionResult> AboutBook(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var book = await context.Book.Include("IdAuthorNavigation").FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View("About", book);
        }

        //GET
        //[Route("{idBook}/Page{page}")]
        public async Task<IActionResult> GetBook(int? idBook, int? page = 1)
        {
            if (idBook == null)
                return NotFound();

            var book = await context.Book.FirstOrDefaultAsync(i => i.Id == idBook);

            if (book == null)
                return NotFound();

            // Если книга найдена, то вернуть текст страницы page

            var glavas = book.ContentBook.Split("Глава").Skip(1).ToArray();

            if (page > glavas.Length)
                return NotFound();
            
            var glava = "<h3>Глава " + glavas[(int)page - 1];

            return View("Get", glava);
        }
    }
}
