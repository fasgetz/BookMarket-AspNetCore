

using BookMarket.Models.DataBase;
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
    }
}