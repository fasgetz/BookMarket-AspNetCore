using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.ViewModels.Profile
{
    public class FavoriteBook
    {
        public string BookName { get; set; }
        public string Author { get; set; }
        public byte[] PosterBook { get; set; }
        public int IdBook { get; set; }
        public int IdAuthor { get; set; }
        public int Id { get; set; }

        public string DescriptionBook { get; set; }
    }
}
