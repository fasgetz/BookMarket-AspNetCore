using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.ViewModels.HomeViewModels
{
    public class IndexBook
    {
        public int Id { get; set; }
        public double RatingBook { get; set; }
        public string Name { get; set; }

        public byte[] PosterBook { get; set; }

        public string AuthorNameFamily { get; set; }
        public int IdAuthor { get; set; }
    }
}
