using AutoFixture;
using BookMarket.Controllers;
using BookMarket.Models.DataBase;
using BookMarket.Models.ViewModels.SearchBook;
using BookMarket.Services.Books;
using BookMarket.Services.Genres;
using BookMarketUnitTests.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace BookMarketUnitTests.Controllers
{
    /// <summary>
    /// Тесты контроллера SearchBook
    /// </summary>
    public class SearchBookControllerTests
    {


        public GenreBook searchGenre(int idGenre)
        {
            IEnumerable<GenreBook> genres = new List<GenreBook>()
            {
                new GenreBook()
                {
                    Id = 1,
                    Name = "Триллер"
                },
                new GenreBook()
                {
                    Id = 2,
                    Name = "Боевик"
                }
            };



            return genres.FirstOrDefault(i => i.Id == idGenre);

        }


        public IEnumerable<BookViewModel> searchBooks(int IdGenre, int page, string word, byte RatingOrdered = 0)
        {
            

            // Фильтрация по параметрам
            var filtered = RepositoryDB.listBookViewModel
                .OrderByDescending(i => RatingOrdered == 1 ? i.RatingBook : 0)
                .Where(i => !string.IsNullOrEmpty(word) ? i.Name.Contains(word) : true)
                .Where(i => IdGenre != 0 ? i.idCategory == IdGenre : true)
                .AsEnumerable();


            return filtered;
        }

        public int maxBookInQuery(int IdGenre, string word)
        {
            // Фильтрация по параметрам
            var filtered = RepositoryDB.listBookViewModel
                .Where(i => !string.IsNullOrEmpty(word) ? i.Name.Contains(word) : true)
                .Where(i => IdGenre != 0 ? i.idCategory == IdGenre : true)
                .Count();

            return filtered;
        }



        #region Index

        [Fact]
        public async void GetIndexViewModel()
        {
            // Arrange
            var someEntity = new Fixture() { RepeatCount = 10 }.Create<IDictionary<CategoryGenreVM, System.Collections.Generic.List<GenreBookVM>>>();

            var bookServiceMock = new Mock<IBookService>();
            var genresServiceMock = new Mock<IGenresService>();
            genresServiceMock.Setup(i => i.getGenresSubCategoriesCounts()).ReturnsAsync(someEntity);

            var controller = new SearchBookController(genresServiceMock.Object, bookServiceMock.Object);

            var result = await controller.Index(null, 1, true) as ViewResult;


            var model = (SearchBookIndexVM)result.Model;

            Assert.IsType<ViewResult>(result);
            Assert.Equal(someEntity.Count, model.CategoryGenres.Count);
        }


        #endregion


        #region LastVisitBooks                

        [Fact]
        public async void lastVisitBooks_UserNotAuth()
        {
            // Arrange
            var bookServiceMock = new Mock<IBookService>();
            var genresServiceMock = new Mock<IGenresService>();


            var controller = new SearchBookController(genresServiceMock.Object, bookServiceMock.Object);

            // Привязываем пользователя к контексту
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    //User = user
                }
            };

            var result = await controller.LastVisitBooks(null);

            Assert.Null(result);
        }


        [Fact]
        public async void lastVisitBooks_UserIsAuth()
        {


            // Arrange
            var bookServiceMock = new Mock<IBookService>();
            var genresServiceMock = new Mock<IGenresService>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "Администратор"),
                new Claim("custom-claim", "example claim value")
            }, "mock"));

            bookServiceMock.Setup(i => i.GetLastVisitBook(user.Identity.Name, 5)).ReturnsAsync(RepositoryDB.lastVisitBooksData);

            var controller = new SearchBookController(genresServiceMock.Object, bookServiceMock.Object);



            // Привязываем пользователя к контексту
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };


            var result = await controller.LastVisitBooks(user.Identity.Name) as PartialViewResult;


            Assert.IsType<PartialViewResult>(result);
            Assert.Equal(RepositoryDB.lastVisitBooksData.Count, ((IDictionary<BookViewModel, visitUser>)result.Model).Count);


        }

        #endregion


        /// <summary>
        /// Поисковая выборка с параметрами
        /// </summary>
        [Theory]
        [InlineData(1, 0, "Книг", 1)]
        [InlineData(0, 0, null, 1)]
        [InlineData(0, 0, null, 0)]
        [InlineData(0, 0, "Книг", 1)]
        private async void getData_getMethod(int IdGenre, int page, string word, byte RatingOrdered = 0)
        {

            // Arrange
            var bookServiceMock = new Mock<IBookService>();
            var genresServiceMock = new Mock<IGenresService>();

            // Arrange mock
            bookServiceMock.Setup(i => i.GetSearchBooks(IdGenre, page, word, RatingOrdered)).ReturnsAsync(searchBooks(IdGenre, page, word, RatingOrdered));
            genresServiceMock.Setup(i => i.FindGenreBook(IdGenre)).ReturnsAsync(searchGenre(IdGenre));
            bookServiceMock.Setup(i => i.getCountsBooks(word, (ushort)IdGenre)).ReturnsAsync(maxBookInQuery(IdGenre, word));

            var controller = new SearchBookController(genresServiceMock.Object, bookServiceMock.Object);

            // Вызываем метод поиска книг
            var result = await controller.getData(IdGenre, page, word, RatingOrdered) as PartialViewResult;

            var model = result.Model as getDataViewModel;

            Assert.Equal(word, model.InputWord);
            Assert.Equal(searchBooks(IdGenre, page, word, RatingOrdered).Count(), model.books.Count());
            Assert.Equal(maxBookInQuery(IdGenre, word), model.MaxCountBooks);

        }
    }
}
