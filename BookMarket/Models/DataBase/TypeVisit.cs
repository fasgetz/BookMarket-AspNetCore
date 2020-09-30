using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.DataBase
{
    /// <summary>
    /// Что посетили (О книге, прочитали книгу, автора)
    /// </summary>
    public class TypeVisit
    {
        int Id { get; set; }
        public string Name { get; set; }
    }
}
