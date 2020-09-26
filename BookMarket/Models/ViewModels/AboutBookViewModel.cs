using BookMarket.Models.DataBase;
using BookMarket.Models.ViewModels.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.ViewModels
{
    public class AboutBookViewModel
    {
        public Book book { get; set; }
        public List<RatingViewModel> RatingsBook { get; set; }
        public Rating MyComment { get; set; }

        public double RatingBook { get; set; }
    }
}
