using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.ViewModels.SearchBook
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public double RatingBook { get; set; }
        public string Name { get; set; }
        public DateTime AddDatabase { get; set; }
        public string CategoryName { get; set; }
        public int idCategory { get; set; }
        public string Description { get; set; }

        public byte[] PosterBook { get; set; }

        public string AuthorNameFamily { get; set; }
        public int IdAuthor { get; set; }
    }
}
