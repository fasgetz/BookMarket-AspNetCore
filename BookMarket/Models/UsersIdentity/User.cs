using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.UsersIdentity
{
    public class User : IdentityUser
    {
        public DateTime dateBirth { get; set; }
        
        public byte[] ProfileImage { get; set; }
    }
}
