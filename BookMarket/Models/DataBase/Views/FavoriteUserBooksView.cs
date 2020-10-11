using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.DataBase.Views
{

    public class FavoriteUserBooksView
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int IdBookFavorite { get; set; }
        public DateTime dateRegistration { get; set; }
    }
}
