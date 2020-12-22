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
        private readonly UsersContext _contextUsers;
        private readonly IProfileService profileService;
        BookMarketContext db;

        public UserService(BookMarketContext _db, UsersContext context, IProfileService profileService)
        {
            this.profileService = profileService;
            this._contextUsers = context;
            this.db = _db;
        }

        public async void AddUserVisit(string IdUser, int IdBook)
        {
            await Task.Run(() =>
            {
                db.UsersVisit.AddAsync(new visitUser() { DateVisit = DateTime.Now, IdBook = IdBook, IdUser = IdUser });
                db.SaveChangesAsync();
            });

        }


        /// <summary>
        /// Получить юзера
        /// </summary>
        /// <param name="UserName">Имя юзера</param>
        /// <returns>Юзера</returns>
        private async Task<User> GetUser(string UserName)
        {
            var user = await _contextUsers.Users.FirstOrDefaultAsync(i => i.Email == UserName);

            return user;
        }

        /// <summary>
        /// Получить пользователей по рейтингу комментариев
        /// </summary>
        /// <param name="count">Количество пользователей</param>
        /// <returns>Список пользователей</returns>
        public async Task<IEnumerable<TopUser>> GetTopUsers(int count)
        {            var users = (await db.Ratings
                .GroupBy(i => i.IdUser)
                .Select(i => new TopUser()
                {
                    Name = i.Key,
                    //Ava = _contextUsers.Users.FirstOrDefault(s => s.Email == i.Key).ProfileImage,
                    Ava = profileService.GetUser(i.Key).Result.ProfileImage,
                    AvgRating = i.Average(i => i.Mark),
                    TotalComments = i.Count()
                })
                .Take(count)
                .ToListAsync())
                .OrderByDescending(i => i.AvgRating).ToList();



            return users;
        }

        /// <summary>
        /// Получить список новых зарегистрированных пользователей
        /// </summary>
        /// <param name="count">Количество пользователей</param>
        /// <returns>Список пользователей</returns>
        public async Task<IEnumerable<TopUser>> GetNewUsers(int count)
        {


            return await _contextUsers.Users
                .OrderByDescending(i => i.dateRegistration)
                .Take(count)
                .Select(i => new TopUser { Name = i.Email, dateRegistration = i.dateRegistration, Ava = i.ProfileImage })
                .ToListAsync();
        }

    }
}
