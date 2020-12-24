using BookMarket.Controllers;
using BookMarket.Models.DataBase;
using BookMarket.Models.ViewModels.HomeViewModels;
using BookMarket.Models.ViewModels.SearchBook;
using BookMarket.Services;
using BookMarket.Services.Books;
using BookMarket.Services.Genres;
using BookMarketUnitTests.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookMarketUnitTests.Controllers
{





    public class HomeControllerTests
    {
        #region Метод NewBook

        // Проверка количества передаваемых элементов в представление newBook
        [Theory]
        [InlineData(3)]
        [InlineData(0)]
        public async void CheckCountElementsTransfetToNewBookPartial(int count)
        {
            // Arrange
            var bookServiceMock = new Mock<IBookService>();

            bookServiceMock.Setup(service => service.GetNewsBooks(count)).ReturnsAsync(RepositoryDB.GetIndexBooks(count));

            var controller = new HomeController(bookServiceMock.Object);

            // Получим представление
            var partialViewResult = await controller.newBooks(count) as PartialViewResult;
            // Получим модель, передаваемую в представление
            var models = partialViewResult.Model as IEnumerable<IndexBook>;

            Assert.Equal(models.Count(), RepositoryDB.GetIndexBooks(count).Count);
        }

        /// <summary>
        /// Проверка передаваемого типа модели в представление
        /// </summary>
        [Fact]
        public async void CheckTransferTypesForNewBookPartialView()
        {
            // Arrange
            var bookServiceMock = new Mock<IBookService>();

            bookServiceMock.Setup(service => service.GetNewsBooks(4)).ReturnsAsync(RepositoryDB.GetIndexBooks(4));

            var controller = new HomeController(bookServiceMock.Object);          

            // Получим представление
            var partialViewResult = await controller.newBooks(4) as PartialViewResult;
            // Получим модель, передаваемую в представление
            var model = partialViewResult.Model;

            // Assert
            Assert.IsAssignableFrom<IEnumerable<IndexBook>>(model);

        }


        /// <summary>
        /// Проверка возвращаемого типа
        /// </summary>
        [Fact]
        public void PartialViewAReturnResultWithNewBook()
        {
            // Arrange
            var bookServiceMock = new Mock<IBookService>();

            var controller = new HomeController(bookServiceMock.Object);

            // Получаем тип
            var result = controller.newBooks();


            // Возвращаемый тип
            Assert.IsType<Task<IActionResult>>(result);
        }

        #endregion

        #region Метод IndexData

        /// <summary>
        /// Проверка передаваемого типа модели в представление
        /// </summary>
        [Fact]
        public async void CheckTransferTypesForIndexData()
        {
            // Arrange
            var genresServiceMock = new Mock<IGenresService>();
            var userServiceMock = new Mock<IUserService>();

            userServiceMock.Setup(service => service.GetTopUsers(5)).ReturnsAsync(RepositoryDB.GetTopUsers(5));
            userServiceMock.Setup(service => service.GetNewUsers(5)).ReturnsAsync(RepositoryDB.GetTopUsers(5));
            genresServiceMock.Setup(service => service.GetGenresSubCategories(4, 3)).ReturnsAsync(RepositoryDB.GetSubGenres(4, 3));

            // Act
            var controller = new HomeController(userServiceMock.Object, genresServiceMock.Object);

            // Проверим модель, которая передается в контроллер
            var partialViewResult = await controller.IndexData(4, 3, 5, 5) as PartialViewResult;
            var model = (IndexViewModel)partialViewResult.Model;

            // Assert

            Assert.IsAssignableFrom<IDictionary<CategoryGenre, System.Collections.Generic.List<GenreBook>>>(model.CategoryGenres);
            Assert.IsAssignableFrom<IEnumerable<TopUser>>(model.topUsers);
            Assert.IsAssignableFrom<IEnumerable<TopUser>>(model.NewUsers);

        }


        /// <summary>
        /// Проверка возвращаемого типа
        /// </summary>
        [Fact]
        public void PartialViewAReturnResultWithIndexData()
        {
            // Arrange
            var bookServiceMock = new Mock<IBookService>();
            var genresServiceMock = new Mock<IGenresService>();
            var userServiceMock = new Mock<IUserService>();

            var controller = new HomeController(genresServiceMock.Object, bookServiceMock.Object, userServiceMock.Object);

            // Получаем тип
            var result = controller.IndexData();


            // Возвращаемый тип
            Assert.IsType<Task<IActionResult>>(result);
        }

        #endregion

        #region Страницы Index - Privacy - Error

        /// <summary>
        /// Проверка возвращаемого типа ViewResult страницы Error
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void ViewResultAReturnWithError()
        {
            // Arrange
            var mock = new Mock<IBookService>();
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.Error();


            // Возвращаемый тип
            Assert.IsType<ViewResult>(result);
        }


        /// <summary>
        /// Проверка возвращаемого типа ViewResult страницы Privacy
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void ViewResultAReturnWithPrivacy()
        {
            // Arrange
            var mock = new Mock<IBookService>();
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.Privacy();


            // Возвращаемый тип
            Assert.IsType<ViewResult>(result);
        }


        /// <summary>
        /// Проверка возвращаемого типа ViewResult страницы Index
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void ViewResultAReturnWithIndex()
        {
            // Arrange
            var mock = new Mock<IBookService>();
            //mock.Setup(service => service.GetTopBooks(4)).ReturnsAsync(Repository.GetIndexBooks());
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.Index();


            // Возвращаемый тип
            Assert.IsType<ViewResult>(result);
        }

        #endregion

        #region LastCommentBook


        /// <summary>
        /// Проверка передаваемого типа в контроллер
        /// </summary>
        /// <returns></returns>
        [Theory]
        [InlineData(3)]
        [InlineData(0)]
        public async Task TypeModelWithLastCommentBookAndCounts(int countBooks)
        {
            // Arrange
            var mock = new Mock<IBookService>();
            mock.Setup(service => service.GetLastCommentaries(countBooks)).ReturnsAsync(RepositoryDB.GetBookViewModel(countBooks));
            var controller = new HomeController(mock.Object);


            // Получаем результат представления
            var partialViewResult = await controller.LastCommentsBook(countBooks) as PartialViewResult;
            var models = partialViewResult.Model as IEnumerable<BookViewModel>;


            Assert.IsAssignableFrom<IEnumerable<BookViewModel>>(models); 
            

            Assert.Equal(models.Count(), RepositoryDB.GetIndexBooks(countBooks).Count);

        }


        /// <summary>
        /// Проверка возвращаемого типа
        /// </summary>
        [Fact]
        public void PartialViewAReturnResultWithLastCommentBook()
        {

            // Arrange
            var mock = new Mock<IBookService>();
            //mock.Setup(service => service.GetTopBooks(4)).ReturnsAsync(Repository.GetIndexBooks());
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.LastCommentsBook();


            // Возвращаемый тип
            Assert.IsType<Task<PartialViewResult>>(result);

        }

        #endregion

        #region Конструкторы


        [Fact]
        public void InitializationConstructor()
        {
            // Arrange
            var bookServiceMock = new Mock<IBookService>();
            var genresServiceMock = new Mock<IGenresService>();
            var userServiceMock = new Mock<IUserService>();

            var controller = new HomeController(genresServiceMock.Object, bookServiceMock.Object, userServiceMock.Object);




            Assert.NotNull(controller.userService);
            Assert.NotNull(controller.genresService);
            Assert.NotNull(controller.serviceBooks);
        }


        #endregion

        #region Метод topBooks

        /// <summary>
        /// Проверка на количество элементов, возвращаемых представлению
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [Theory]
        [InlineData(5)]
        [InlineData(3)]
        public async Task BooksModelCountsWithTopBooks(int count)
        {
            // Arrange
            var mock = new Mock<IBookService>();
            mock.Setup(service => service.GetTopBooks(count)).ReturnsAsync(RepositoryDB.GetIndexBooks(count));
            var controller = new HomeController(mock.Object);

            // Проверим модель, которая передается в контроллер
            var partialViewResult = await controller.topBooks(count) as PartialViewResult;

            var models = partialViewResult.Model as List<IndexBook>;


            Assert.Equal(models.Count, RepositoryDB.GetIndexBooks(count).Count);
        }


        /// <summary>
        /// Проверка на передачу модели в представление контроллера
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ListBooksModelWithTopBooks()
        {
            // Arrange
            var mock = new Mock<IBookService>();
            mock.Setup(service => service.GetTopBooks(4)).ReturnsAsync(RepositoryDB.GetIndexBooks());
            var controller = new HomeController(mock.Object);

            // Проверим модель, которая передается в контроллер
            var partialViewResult = await controller.topBooks() as PartialViewResult;

            Assert.IsAssignableFrom<List<IndexBook>>(partialViewResult.Model);
        }


        /// <summary>
        /// Проверка возвращаемого типа
        /// </summary>
        [Fact]
        public void PartialViewAReturnResultWithTopBooks()
        {

            // Arrange
            var mock = new Mock<IBookService>();
            mock.Setup(service => service.GetTopBooks(4)).ReturnsAsync(RepositoryDB.GetIndexBooks());
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.topBooks();


            // Возвращаемый тип
            Assert.IsType<Task<IActionResult>>(result);

        }

        #endregion


    }

}
