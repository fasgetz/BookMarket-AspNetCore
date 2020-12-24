using BookMarket.Controllers;
using BookMarket.Models.DataBase;
using BookMarket.Models.ViewModels.SearchBook;
using BookMarket.Services.Books;
using BookMarket.Services.Genres;
using BookMarketUnitTests.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
