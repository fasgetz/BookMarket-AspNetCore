

using BookMarket.Models.DataBase;
using BookMarket.Models.ViewModels.SearchBook;
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


        /// <summary>
        /// ������� ������ � ���������� � ����������� ����
        /// </summary>
        /// <returns></returns>
        public Task<IDictionary<CategoryGenreVM, System.Collections.Generic.List<GenreBookVM>>> getGenresSubCategoriesCounts();

        /// <summary>
        /// ����� ����� ����� �� ������
        /// </summary>
        /// <param name="idGenre">����� �����</param>
        /// <returns>���� �����</returns>
        public Task<GenreBook> FindGenreBook(int idGenre);
    }
}