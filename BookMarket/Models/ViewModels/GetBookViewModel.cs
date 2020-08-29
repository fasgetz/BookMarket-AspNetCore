using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.ViewModels
{
    public class GetBookViewModel
    {
        public string content { get; set; } // Содержимое страницы
        public int IdBook { get; set; } // Айди книги
        public int thisPage { get; set; } // Текущая страница
        public int CountPage { get; set; } // Количество страниц
    }
}
