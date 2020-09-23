using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.ViewModels.SearchBook
{
    public class GenreBookVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountBooks { get; set; }
    }
}
