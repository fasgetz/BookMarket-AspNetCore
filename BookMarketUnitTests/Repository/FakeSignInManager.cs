using BookMarket.Models.UsersIdentity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookMarketUnitTests.Repository
{
    public class FakeSignInManager : SignInManager<User>
    {
        public FakeSignInManager(UserManager<User> userManager)
            : base(userManager,
                  new HttpContextAccessor(),
                  new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<ILogger<SignInManager<User>>>().Object
                  , null, null)
        {
        }


    }
}
