using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.DataBase
{
    public class Rating
    {
        public Rating()
        {
            DateCreated = DateTime.Now;
        }
        public int Id { get; set; }

        // Оценка книги 1..10
        public byte Mark { get; set; }



        // Айди пользователя, который поставил оценку

        public string IdUser { get; set; }

        // Комментарий книги
        public string Comment { get; set; }

        // Дата создания комментария
        public DateTime DateCreated { get; set; }

        // Книга
        public int IdBook { get; set; } // Айди книги
        public virtual Book BookRating { get; set; }
    }
}
