using System;
using System.Collections.Generic;

namespace BookMarket.Models.DataBase
{
    public partial class GenreBook
    {
        public GenreBook()
        {
            Book = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Book> Book { get; set; }
    }
}
