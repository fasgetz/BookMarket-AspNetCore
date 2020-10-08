using BookMarket.Models.DataBase;
using BookMarket.Models.ViewModels.SearchBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.ViewModels.Authors
{
    public class DetailViewModel
    {
        public Author author { get; set; }
        public List<BookViewModel> books { get; set; }
    }
}
