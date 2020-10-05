using BookMarket.Models.DataBase;
using BookMarket.Models.UsersIdentity;
using BookMarket.Models.ViewModels.HomeViewModels;
using BookMarket.Services.Profile;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Services
{
    public class UserService : IUserService
    {
        BookMarketContext db;
        private readonly IProfileService profileService;

        public UserService(BookMarketContext _db, IProfileService profileService)
        {
            this.db = _db;
            this.profileService = profileService;
        }

        public async void AddUserVisit(string IdUser, int IdBook)
        {
            db.UsersVisit.AddAsync(new visitUser() { DateVisit = DateTime.Now, IdBook = IdBook, IdUser = IdUser });
            db.SaveChanges();
        }



        /// <summary>
        /// Получить пользователей по рейтингу комментариев
        /// </summary>
        /// <param name="count">Количество пользователей</param>
        /// <returns>Список пользователей</returns>
        public async Task<IList<TopUser>> GetTopUsers(int count)
        {


            var users = (await db.Ratings
                .GroupBy(i => i.IdUser)
                .Select(i => new TopUser()
                {
                    Name = i.Key,
                    Ava = profileService.GetUser(i.Key).Result.ProfileImage,
                    AvgRating = i.Average(i => i.Mark),
                    TotalComments = i.Count()
                })
                .Take(count)
                .ToListAsync())
                .OrderByDescending(i => i.AvgRating).ToList();



            return users;
        }
    }
}
