

using BookMarket.Models.DataBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMarket.Services.Genres
{
    public interface IGenresService
    {
        /// <summary>
        /// ������� ��������� ������
        /// </summary>
        /// <param name="counts">����������</param>
        /// <param name="countSubCategory">���������� ������������</param>        
        /// <returns></returns>
        public Task<IDictionary<CategoryGenre, System.Collections.Generic.List<GenreBook>>> GetGenresSubCategories (int countCategory = 8, int countSubCategory = 3);
    }
}