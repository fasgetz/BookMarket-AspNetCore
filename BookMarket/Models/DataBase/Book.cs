using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMarket.Models.DataBase
{
    public partial class Book
    {
        public Book()
        {
            ChapterBook = new HashSet<ChapterBook>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? IdCategory { get; set; }
        public int? IdAuthor { get; set; }
        public byte[] PosterBook { get; set; }

        public DateTime AddDatabase { get; set; }

        public virtual Author IdAuthorNavigation { get; set; }
        public virtual GenreBook IdCategoryNavigation { get; set; }
        public virtual ICollection<ChapterBook> ChapterBook { get; set; }
    }
}
