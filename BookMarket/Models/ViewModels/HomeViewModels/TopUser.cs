using BookMarket.Models.UsersIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.ViewModels.HomeViewModels
{
    /// <summary>
    /// Рейтинг пользователь
    /// </summary>
    public class TopUser
    {
        public string Name { get; set; }
        public byte[] Ava { get; set; }
        public int TotalComments { get; set; }
        public double AvgRating { get; set; }
        public DateTime dateRegistration { get; set; }

    }
}
