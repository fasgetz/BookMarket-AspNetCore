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
            await db.UsersVisit.AddAsync(new visitUser() { DateVisit = DateTime.Now, IdBook = IdBook, IdUser = IdUser });
            await db.SaveChangesAsync();
        }
    }
}
