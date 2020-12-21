using BookMarket.Controllers;
using BookMarket.Models.ViewModels.HomeViewModels;
using BookMarket.Services.Books;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

    [TestFixture]
    public class HomeControllerTests
    {

        [SetUp]
        public void Setup()
        {

        }

        #region Метод topBooks

        /// <summary>
        /// Проверка на количество элементов, возвращаемых представлению
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [TestCase(3)]
        [TestCase(5)]
        public async Task BooksModelCountsWithTopBooks(int count)
        {
            // Arrange
            var mock = new Mock<IBookService>();
            mock.Setup(service => service.GetTopBooks(count)).ReturnsAsync(Repository.GetIndexBooks(count));
            var controller = new HomeController(mock.Object);

            // Проверим модель, которая передается в контроллер
            var partialViewResult = await controller.topBooks(count) as PartialViewResult;

            var models = partialViewResult.Model as List<IndexBook>;


            Assert.AreEqual(models.Count, Repository.GetIndexBooks(count).Count);
        }


        /// <summary>
        /// Проверка на передачу модели в представление контроллера
        /// </summary>
        /// <returns></returns>
        [Test]       
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
        [Test]
        public void PartialViewAReturnResultWithTopBooks()
        {
            
            // Arrange
            var mock = new Mock<IBookService>();
            mock.Setup(service => service.GetTopBooks(4)).ReturnsAsync(Repository.GetIndexBooks());
            var controller = new HomeController(mock.Object);

            // Act
            var result = controller.topBooks();


            // Возвращаемый тип
            Assert.IsInstanceOf<Task<IActionResult>>(result);

        }

        #endregion


    }
}
