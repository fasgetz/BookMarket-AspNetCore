using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.ViewModels.Books
{
    public class RatingViewModel
    {
        // Оценка книги 1..10
        public byte Mark { get; set; }



        // Имя пользователя, который поставил оценку

        public string UserName { get; set; }

        // Комментарий книги
        public string Comment { get; set; }

        // Дата создания комментария
        public DateTime DateCreated { get; set; }
    }
}
