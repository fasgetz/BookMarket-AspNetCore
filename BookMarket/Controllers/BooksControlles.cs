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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BookMarket.Models.UsersIdentity;
using BookMarket.Models.ViewModels.Books;
using BookMarket.Services;

namespace BookMarket.Controllers
{

    //[Route("book")]

    public class BooksController : Controller
    {
        private readonly IUserService userService;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ILogger<BooksController> _logger;

        BookMarketContext context;
        public BooksController(ILogger<BooksController> logger, IWebHostEnvironment appEnvironment, BookMarketContext context, IUserService _userSerivce)
        {
            this.userService = _userSerivce;
            _logger = logger;
            _appEnvironment = appEnvironment;

            this.context = context;
        }

        #region Методы контроллера

        #region Редактировать комментарий

        [HttpGet]
        public IActionResult EditCommentary(int IdBook, string comment, int IdComment)
        {
            EditCommentVM vm = new EditCommentVM()
            {
                IdComment = IdComment,
                IdBook = IdBook,
                comment = comment
            };

            return PartialView(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditComment(EditCommentVM vm)
        {

            if (ModelState.IsValid && vm.Mark != 0)
            {
                // Если юзер авторизован, то оставить комментарий
                if (User.Identity.IsAuthenticated)
                {
                    // Находим комментарий
                    var comment = await context.Ratings.FirstOrDefaultAsync(i => i.Id == vm.IdComment); 

                    // Если комментарий найден, то изменить его
                    if (comment != null)
                    {
                        comment.Mark = (byte)vm.Mark;
                        comment.Comment = vm.comment;

                        context.SaveChanges();
                        return Redirect($"~/Books/AboutBook?id={vm.IdBook}");
                    }

                }
            }






            return await AboutBook(vm.IdBook);
        }

        #endregion

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

        /// <summary>
        /// Добавить комментарий к книге
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCommentaryBook(AddCommentaryOnBookViewModel vm)
        {
            if (ModelState.IsValid && vm.rating != 0)
            {
                // Если юзер авторизован, то оставить комментарий
                if (User.Identity.IsAuthenticated)
                {


                    // Далее добавляем комментарий в бд
                    await context.Ratings.AddAsync(new Rating()
                    {
                        IdBook = vm.IdBook,
                        IdUser = User.Identity.Name,
                        Mark = (byte)vm.rating,
                        Comment = vm.Commentary
                    });
                    await context.SaveChangesAsync();


                    return Redirect($"~/Books/AboutBook?id={vm.IdBook}");;
                }
            }






            return await AboutBook(vm.IdBook);
        }




        /// <summary>
        /// О книге
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AboutBook(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AboutBookViewModel vm = new AboutBookViewModel();

            await Task.Run(() =>
            {
                // Получаем пользователя, если он авторизован
                if (User.Identity.IsAuthenticated)
                {
                    userService.AddUserVisit(User.Identity.Name, (int)id);
                }
            });

            
            
            vm.book = await context.Book.Include("IdAuthorNavigation").FirstOrDefaultAsync(m => m.Id == id);

            // Комментарии книги
            vm.RatingsBook = await context.Ratings
                .Where(i => i.IdBook == id)
                .Select(i => new RatingViewModel() { Comment = i.Comment, DateCreated = i.DateCreated, Mark = i.Mark, UserName = i.IdUser }).ToListAsync(); // Список комментариев книги

            // Если у книги есть комментарии, то посчитай среднюю оценку
            if (vm.RatingsBook.Count != 0)
                vm.RatingBook = vm.RatingsBook.Average(i => i.Mark);

            // Если юзер авторизован, то узнать возможность комментирования
            if (User.Identity.IsAuthenticated)
            {
                // Получаем комментарий
                vm.MyComment = await context.Ratings
                    .FirstOrDefaultAsync(i => i.IdBook == id && i.IdUser == User.Identity.Name);
                
                if (vm.MyComment != null)
                    // Убираем комментарии пользователя
                    vm.RatingsBook = vm.RatingsBook.Where(i => i.UserName != vm.MyComment.IdUser).ToList();
            }
                

            



            if (vm.book == null)
            {
                return NotFound();
            }

            return View("About", vm);
        }



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

            // Логируем просмотр книги
            await Task.Run(() =>
            {
                // Получаем пользователя, если он авторизован
                if (User.Identity.IsAuthenticated)
                {
                    userService.AddUserVisit(User.Identity.Name, (int)idBook);
                }
            });

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
        [Authorize(Roles = "Администратор")]
        public IActionResult AddBook([Bind("NameBook, PosterBook, idAuthor, idGenre, XMLBook, DescriptionBook")]AddBookViewModel model)
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
                    IdCategory = model.idGenre,
                    PosterBook = imageData,
                    Name = model.NameBook,
                    IdAuthor = (int)model.idAuthor,
                    Description = model.DescriptionBook,
                    AddDatabase = DateTime.Now
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
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> AddBook()
        {
            // Прогружаем список авторов
            IEnumerable<Author> Authors = await context.Author.Select(i => new Author {  Name = i.Name, Family = i.Family, Id = i.Id }).ToListAsync();
            IEnumerable<GenreBook> Genres = await context.GenreBooks.Select(i => new GenreBook { Id = i.Id, Name = i.Name }).ToListAsync();

            ViewBag.genresList = new SelectList(Genres, "Id", "Name");
            ViewBag.list = new SelectList(Authors, "Id", "NameFamily");

            return View("AddBook");
        }


  

        #endregion
    }
}
