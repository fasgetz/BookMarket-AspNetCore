using BookMarket.Models.DataBase;
using BookMarket.Models.ViewModels.HomeViewModels;
using BookMarket.Models.ViewModels.SearchBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookMarketUnitTests.Repository
{
    public static class RepositoryDB
    {

        #region SearchBookControllerTest

        public static IEnumerable<BookViewModel> listBookViewModel = new List<BookViewModel>()
            {
                new BookViewModel()
                {
                    Name = "Книга",
                    idCategory = 1,
                    RatingBook = 4,
                    Description = "About book ..."
                },
                new BookViewModel()
                {
                    Name = "Книжка",
                    idCategory = 2,
                    RatingBook = 5,
                },
                new BookViewModel()
                {
                    Name = "Книга 2",
                    idCategory = 2,
                    RatingBook = 6.8,
                },
                new BookViewModel()
                {
                    Name = "Книжка 2",
                    idCategory = 2,
                    RatingBook = 9,
                },
                new BookViewModel()
                {
                    Name = "Книжка 3",
                    idCategory = 1,
                    RatingBook = 8,
                }
            };

        #endregion


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


        public static IEnumerable<BookViewModel> GetBookViewModel(int counts = 0)
        {
            var list = new List<BookViewModel>()
            {
                new BookViewModel(),
                new BookViewModel(),
                new BookViewModel(),
                new BookViewModel(),
                new BookViewModel()
            };

            if (counts > 0)
                list = list.Take(counts).ToList();

            return list;
        }


        public static IDictionary<CategoryGenre, System.Collections.Generic.List<GenreBook>> GetSubGenres(int genresCount = 0, int subGenresCount = 0)
        {
            var genres = new Dictionary<CategoryGenre, System.Collections.Generic.List<GenreBook>>();

            genres.Add(new CategoryGenre(), new List<GenreBook>());
            genres.Add(new CategoryGenre(), new List<GenreBook>());
            genres.Add(new CategoryGenre(), new List<GenreBook>());
            genres.Add(new CategoryGenre(), new List<GenreBook>());
            genres.Add(new CategoryGenre(), new List<GenreBook>());



            return genres;
        }

        public static IEnumerable<TopUser> GetTopUsers(int count = 10)
        {
            var users = new List<TopUser>()
            {
                new TopUser(),
                new TopUser(),
                new TopUser(),
                new TopUser(),
                new TopUser()
            };


            return users;
        }

    }
}
