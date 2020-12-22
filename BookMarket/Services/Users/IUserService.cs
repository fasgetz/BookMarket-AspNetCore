using BookMarket.Models.ViewModels.HomeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Services
{
    public interface IUserService
    {
        void AddUserVisit(string IdUser, int IdBook);


        /// <summary>
        /// Получить пользователей по рейтингу комментариев
        /// </summary>
        /// <param name="count">Количество пользователей</param>
        /// <returns>Список пользователей</returns>
        Task<IEnumerable<TopUser>> GetTopUsers(int count);

        /// <summary>
        /// Получить список новых зарегистрированных пользователей
        /// </summary>
        /// <param name="count">Количество пользователей</param>
        /// <returns>Список пользователей</returns>
        Task<IEnumerable<TopUser>> GetNewUsers(int count);

    }
}
