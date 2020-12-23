using AutoFixture;
using BookMarket.Controllers;
using BookMarket.Models.UsersIdentity;
using BookMarket.Models.ViewModels.UsersIdentity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookMarketUnitTests.Controllers
{

    public class FakeUserManager : UserManager<User>
    {
        public FakeUserManager()
            : base(new Mock<IUserStore<User>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<IPasswordHasher<User>>().Object,
                  new IUserValidator<User>[0],
                  new IPasswordValidator<User>[0],
                  new Mock<ILookupNormalizer>().Object,
                  new Mock<IdentityErrorDescriber>().Object,
                  new Mock<IServiceProvider>().Object,
                  new Mock<ILogger<UserManager<User>>>().Object)
        { }
    }

    public class FakeSignInManager : SignInManager<User>
    {
        public FakeSignInManager(UserManager<User> userManager)
            : base(userManager,
                  new HttpContextAccessor(),
                  new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<ILogger<SignInManager<User>>>().Object
                  , null, null)
        { }

        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) =>
            {
                ls.Add(x);
            });
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);

            return mgr;
        }
    }


    public class AccountControllerTests
    {



        #region Свойства

        Mock<UserManager<User>> userManager;
        Mock<FakeSignInManager> signInManager;

        /// <summary>
        /// Список пользователей юзер менеджера
        /// </summary>
        private List<User> _users = new List<User>
        {
              new User() { Name = "testUser", Id = "123" }
        };

        #endregion


        public AccountControllerTests() 
        {

            userManager = FakeSignInManager.MockUserManager<User>(_users);
            signInManager = new Mock<FakeSignInManager>(userManager.Object);

        }



        /// <summary>
        /// Проверка на правильность ввод совпадающих паролей
        /// </summary>
        [Fact]
        public async void testing()
        {
            signInManager.Setup(
                    x => x.SignInAsync(It.IsAny<User>(), It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Failed));


            var controller = new AccountController(userManager.Object, signInManager.Object);


            RegisterUserViewModel vm = new RegisterUserViewModel()
            {
                Email = "test@email.ru",
                Password = "pass123QWE",
                PasswordConfirm = "pass123QWEbad",
                DateBirth = DateTime.Now
            };


            //var res = controller.Register(vm) as IActionResult;
            // Результат контроллера
            var result = await controller.Register(vm) as ViewResult;

            var error = result.ViewData.ModelState.FindKeysWithPrefix("SamePassword").FirstOrDefault();




            Assert.NotNull(error.Value);
            Assert.Equal("SamePassword", error.Key);


        }





        #region Old Tests





/*        [Fact]
        public async void AuthUser()
        {



            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "Администратор"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));




            var userMgr = MockUserManager<User>(_users).Object;

            var newUser = new User()
            {
                Name = "test",
                Email = "test@mail.ru"
            };


            var password = "P@ssw0rd!";

            var result = await userMgr.CreateAsync(newUser, password);

            //result.Succeeded;



            Assert.Equal(2, _users.Count);
            //userMgr.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);



            *//*            var controller = new AccountController()
                        {
                            HttpContext
                        }*//*

            //AccountController controller = new AccountController(userMgr.Object)

            //var MyUser = await userMgr.FindByNameAsync(User.Identity.Name);
        }*/

        #endregion



    }
}
