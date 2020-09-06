using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.DataBase
{
    public class ChapterBook
    {

        public int Id { get; set; }
        public int NumberChapter { get; set; }
        public string ChapterName { get; set; }
        public string ChapterContent { get; set; }
        public int? IdBook { get; set; }

        public virtual Book IdBookNavigation { get; set; }

    }
}
