using BookMarket.Models.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.ViewModels.SearchBook
{
    public class getDataViewModel
    {
        public string InputWord { get; set; } // Слово поиска
        public int page { get; set; } // Текущая страница
        public int MaxCountBooks { get; set; } // Количество книг
        public string GenreName { get; set; } // Имя жанра
        public int IdGenre { get; set; } // Айди жанра
        public List<Book> books { get; set; }
    }
}
