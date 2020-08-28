using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMarket.Models.DataBase
{
    public partial class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] ContentBook { get; set; }

        [Column(TypeName = "image")]
        public byte[] PosterBook { get; set; }
        public int? IdCategory { get; set; }
        public int? IdAuthor { get; set; }

        public virtual Author IdAuthorNavigation { get; set; }
        public virtual CategoryBook IdCategoryNavigation { get; set; }
    }
}
