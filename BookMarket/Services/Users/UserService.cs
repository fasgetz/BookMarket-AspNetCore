using BookMarket.Models.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Services
{
    public class UserService : IUserService
    {
        BookMarketContext db;

        public UserService(BookMarketContext _db)
        {
            this.db = _db;
        }

        public async void AddUserVisit(string IdUser, int IdBook)
        {
            db.UsersVisit.AddAsync(new visitUser() { DateVisit = DateTime.Now, IdBook = IdBook, IdUser = IdUser });
            db.SaveChanges();
        }
    }
}
