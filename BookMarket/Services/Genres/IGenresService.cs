

using BookMarket.Models.DataBase;
using BookMarket.Models.ViewModels.SearchBook;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMarket.Services.Genres
{
    public interface IGenresService
    {
        /// <summary>
        /// Выборка категорий жанров
        /// </summary>
        /// <param name="counts">Количество</param>
        /// <param name="countSubCategory">Количество подкатегорий</param>        
        /// <returns></returns>
        public Task<IDictionary<CategoryGenre, System.Collections.Generic.List<GenreBook>>> GetGenresSubCategories (int countCategory = 8, int countSubCategory = 3);


        /// <summary>
        /// Выборка жанров с поджанрами с количеством книг
        /// </summary>
        /// <returns></returns>
        public Task<IDictionary<CategoryGenreVM, System.Collections.Generic.List<GenreBookVM>>> getGenresSubCategoriesCounts();

        /// <summary>
        /// Поиск жанра книги по номеру
        /// </summary>
        /// <param name="idGenre">номер жанра</param>
        /// <returns>Жанр книги</returns>
        public Task<GenreBook> FindGenreBook(int idGenre);
    }
}