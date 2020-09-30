using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Services
{
    public interface IUserService
    {
        void AddUserVisit(string IdUser, int IdBook);
    }
}
