using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookMarket.Models.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using BookMarket.Models.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BookMarket.Controllers
{

    //[Route("book")]
    public class BooksController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ILogger<BooksController> _logger;
        BookMarketContext context;
        public BooksController(ILogger<BooksController> logger, IWebHostEnvironment appEnvironment, BookMarketContext context)
        {
            _logger = logger;
            _appEnvironment = appEnvironment;

            this.context = context;
        }

        #region Методы контроллера

        #region Комментарии о книге

        public IActionResult GetCommentaryBook(int idBook)
        {
            return PartialView("GetDataBook");
        }

        #endregion


        // GET: Books 
        public async Task<IActionResult> Index()
        {
            return View(await Task.Run(() => context.Book.OrderByDescending(i => i.Id).ToList()));
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


        //[HttpGet]
        //public async Task<JsonResult> GetBookDataJSON(int idBook, int page)
        //{
        //    var data = await context.ChapterBook.FirstOrDefaultAsync(i => i.IdBook == idBook && i.NumberChapter == page);

        //    return new JsonResult(data.ChapterContent);
        //}

        [HttpGet]
        public IActionResult GetDataBook(int idBook, int page)
        {            
            GetBookViewModel vm = new GetBookViewModel()
            {
                thisPage = (int)page,
                IdBook = (int)idBook,
                CountPage = context.ChapterBook.Where(i => i.IdBook == idBook).Count(),
                content = $"{context.ChapterBook.FirstOrDefault(i => i.IdBook == idBook && i.NumberChapter == page).ChapterContent}"
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
            IEnumerable<ChapterBook> Chapters = context.ChapterBook.Where(i => i.IdBook == idBook).OrderBy(i => i.NumberChapter).Select(i => new ChapterBook { ChapterName = i.ChapterName, NumberChapter = i.NumberChapter}).ToList();




            ViewBag.list = Chapters;
            ViewBag.idBook = idBook;



            return View("Get");
        }
        #endregion

        #region AddBookView


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBook([Bind("NameBook, PosterBook, idAuthor, XMLBook, DescriptionBook")]AddBookViewModel model)
        {
            Book book = null;

            if (ModelState.IsValid)
            {
                byte[] imageData = null;

                //считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(model.PosterBook.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)model.PosterBook.Length);
                }

                book = new Book()
                {
                    PosterBook = imageData,
                    Name = model.NameBook,
                    IdAuthor = (int)model.idAuthor,
                    Description = model.DescriptionBook                   
                };

                // Считывание XML файла содержимое книги и парсинг
                using (System.IO.StreamReader stream = new System.IO.StreamReader(model.XMLBook.OpenReadStream()))
                {
                    int numberChapter = 0;

                    var xdoc = XDocument.Parse(stream.ReadToEnd());
                    var root = xdoc.Root.Elements();
                    
                    foreach (var item in root)
                    {
                        ChapterBook chapter = new ChapterBook();

                        // Присваиваем заголовки
                        chapter.ChapterName = (item.Element("title").LastNode as XElement).Value;
                        chapter.NumberChapter = ++numberChapter;

                        // Формируем текст главы
                        foreach (var node in item.Nodes().Skip(1).ToList())
                        {
                            chapter.ChapterContent += node.ToString() + "\n";
                        }

                        book.ChapterBook.Add(chapter);
                    }


                    context.Book.Add(book);
                    context.SaveChanges();

                    return RedirectToAction("Index");
                }

            }
            else
            {
                // LOAD DATA
            }

            //View()
            return View("AddBook");
        }

        // GET: 
        [HttpGet]
        public IActionResult AddBook()
        {
            // Прогружаем список авторов
            IEnumerable<Author> Authors = context.Author.Select(i => new Author {  Name = i.Name, Family = i.Family, Id = i.Id }).ToList();

            ViewBag.list = new SelectList(Authors, "Id", "NameFamily");

            return View("AddBook");
        }


  

        #endregion
    }
}
