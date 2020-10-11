using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.DataBase
{
    public partial class FavoriteUserBook
    {
        /// <summary>
        /// Айди записи
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Айди книги
        /// </summary>
        public int IdBookFavorite { get; set; }
        public virtual Book BookFavorite { get; set; }

        /// <summary>
        /// Айди пользователя
        /// </summary>
        public string UserId { get; set; }
    }
}
