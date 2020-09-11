using System;
using System.Collections.Generic;

namespace BookMarket.Models.DataBase
{
    public partial class Author
    {
        #region

        public string NameFamily => $"{Name} {Family}";

        #endregion
        public Author()
        {
            Book = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string Surname { get; set; }
        public DateTime? DateBirth { get; set; }

        public virtual ICollection<Book> Book { get; set; }
    }
}
