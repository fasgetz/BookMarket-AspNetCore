

using BookMarket.Models.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Services.Genres
{
    public class GenresService : IGenresService
    {
        BookMarketContext db;

        public GenresService(BookMarketContext db)
        {
            this.db = db;
        }


        /// <summary>
        /// ������� ��������� ������
        /// </summary>
        /// <param name="counts">����������</param>
        /// <param name="countSubCategory">���������� ������������</param>        
        /// <returns></returns>
        public async Task<IDictionary<CategoryGenre, System.Collections.Generic.List<GenreBook>>> GetGenresSubCategories(int countCategory = 8, int countSubCategory = 3)
        {

            // �������
            var categoryGenres = await db.GenreCategory
                                .Include("GenresBook")
                                .Take(countCategory)
                                .ToDictionaryAsync(i => new CategoryGenre() { Id = i.Id, Name = i.Name }, s => s.GenresBook.Take(countSubCategory).ToList());


            return categoryGenres;

        }

    }
}