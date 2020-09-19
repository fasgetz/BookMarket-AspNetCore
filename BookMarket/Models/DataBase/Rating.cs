using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.DataBase
{
    public class Rating
    {
        public int Id { get; set; }

        // Оценка книги 1..10
        public byte Mark { get; set; }

        // Айди книги оценки
        public int IdBook { get; set; }
        // Айди пользователя, который поставил оценку

        public int IdUser { get; set; }

        // Комментарий книги
        public string Comment { get; set; }
    }
}
