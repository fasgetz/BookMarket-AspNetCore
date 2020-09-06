using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookMarket.Models.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using BookMarket.Models.ViewModels;
using System.Collections.Generic;

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


        [HttpGet]
        public async Task<JsonResult> GetBookDataJSON(int idBook, int page)
        {
            var data = await context.ChapterBook.FirstOrDefaultAsync(i => i.IdBook == idBook && i.NumberChapter == page);

            return new JsonResult(data.ChapterContent);
        }

        [HttpGet]
        public IActionResult GetDataBook(int idBook, int page)
        {            
            GetBookViewModel vm = new GetBookViewModel()
            {
                thisPage = (int)page,
                IdBook = (int)idBook,
                CountPage = context.ChapterBook.Where(i => i.IdBook == idBook).Count(),
                content = $"{context.ChapterBook.FirstOrDefault(i => i.NumberChapter == page).ChapterContent}"
            };

            return PartialView("GetDataBook", vm);
        }


        [HttpGet]
        public async Task<IActionResult> GetBook(int? idBook)
        {
            if (idBook == null)
                return NotFound();

            var book = await context.Book.FirstOrDefaultAsync(i => i.Id == idBook);

            if (book == null)
                return NotFound();

            // Прогружаем список глав книги
            IEnumerable<ChapterBook> Chapters = context.ChapterBook.Where(i => i.IdBook == idBook).Select(i => new ChapterBook { ChapterName = i.ChapterName, NumberChapter = i.NumberChapter}).ToList();




            ViewBag.list = Chapters;
            ViewBag.idBook = idBook;



            return View("Get");
        }
    }
}
