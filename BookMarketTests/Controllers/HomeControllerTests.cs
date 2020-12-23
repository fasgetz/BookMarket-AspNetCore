using BookMarket.Controllers;
using BookMarket.Models.ViewModels.HomeViewModels;
using BookMarket.Models.ViewModels.SearchBook;
using BookMarket.Services;
using BookMarket.Services.Books;
using BookMarket.Services.Genres;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookMarketTests.Controllers
{



    public static class Repository
    {
        public static List<IndexBook> GetIndexBooks(int counts = 0)
        {
            List<IndexBook> list = new List<IndexBook>()
            {
                new IndexBook(),
                new IndexBook(),
                new IndexBook(),
                new IndexBook(),
                new IndexBook()
            };


            return list.Take(counts != 0 ? counts : list.Count).ToList();
        }


        public static ICollection<BookViewModel> GetBookViewModel(int counts = 0)
        {
            var list = new List<BookViewModel>()
            {
                new BookViewModel(),
                new BookViewModel(),
                new BookViewModel(),
                new BookViewModel(),
                new BookViewModel()
            }.Take(counts != 0 ? counts : int.MaxValue);
            
            return list.ToList();
        }
    }


    public class HomeControllerTests
    {




        #region LastCommentBook


        /// <summary>
        /// Проверка передаваемого типа в контроллер
        /// </summary>
        /// <returns></returns>
        [InlineData(3)]
        public async Task TypeModelWithLastCommentBook(int countBooks)
        {

            
            var data = Repository.GetBookViewModel();
            var test = data.Batch(5);



            // Arrange
            var mock = new Mock<IBookService>();
            mock.Setup(service => service.GetLastCommentaries(countBooks)).ReturnsAsync(data);
            var controller = new HomeController(mock.Object);

            
            var partialViewResult = await controller.LastCommentsBook(countBooks) as PartialViewResult;



            Assert.IsAssignableFrom<ICollection<BookViewModel>>(partialViewResult.Model);

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
        [InlineData(5)]
        [InlineData(3)]
        public async Task BooksModelCountsWithTopBooks(int count)
        {
            // Arrange
            var mock = new Mock<IBookService>();
            mock.Setup(service => service.GetTopBooks(count)).ReturnsAsync(Repository.GetIndexBooks(count));
            var controller = new HomeController(mock.Object);

            // Проверим модель, которая передается в контроллер
            var partialViewResult = await controller.topBooks(count) as PartialViewResult;

            var models = partialViewResult.Model as List<IndexBook>;


            Assert.Equal(models.Count, Repository.GetIndexBooks(count).Count);
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
            mock.Setup(service => service.GetTopBooks(4)).ReturnsAsync(Repository.GetIndexBooks());
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
            mock.Setup(service => service.GetTopBooks(4)).ReturnsAsync(Repository.GetIndexBooks());
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.topBooks();


            // Возвращаемый тип
            Assert.IsType<Task<IActionResult>>(result);

        }

        #endregion


    }
}
