using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.DataBase
{
    /// <summary>
    /// Информация о посещении пользователем книг / чтения книг
    /// </summary>
    public class visitUser
    {
        public int Id { get; set; }
        public DateTime DateVisit { get; set; }
        
        public string IdUser { get; set; }

        // Ссылка на книгу
        public int IdBook { get; set; }
        public virtual Book book { get; set; }
    }
}
