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

    public class FakeUserManager
    {


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

            userManager = FakeUserManager.MockUserManager<User>(_users);
            signInManager = new Mock<FakeSignInManager>(userManager.Object);

        }


        #region get method Register


        /// <summary>
        /// Проверка возвращаемости страницы, если пользователь не авторизован
        /// </summary>
        [Fact]
        public void NotAuthUserRegisterAction()
        {
            var controller = new AccountController(userManager.Object, signInManager.Object);

            // Устанавливаем контекст
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
 
                }
            };

            // Получаем результат перехода на страницу регистрации без аутентификации
            var result = controller.Register() as IActionResult;


            // Возвращаемый тип
            Assert.IsType<ViewResult>(result);

        }

        /// <summary>
        /// Проверка перехода на страницу регистрации, если пользователь авторизован
        /// </summary>
        [Fact]
        public void AuthUserRegisterAction()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "Администратор"),
                new Claim("custom-claim", "example claim value")                
            }, "mock"));
 
            var controller = new AccountController(userManager.Object, signInManager.Object);

            // Привязываем пользователя к контексту
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };

            var result = controller.Register() as RedirectToActionResult;


            // Устанавливаем фейкового авторзиованного пользователя



            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
        }

        #endregion

        #region post method Register

        /// <summary>
        /// Проверка на правильность ввод совпадающих паролей
        /// </summary>
        [Fact]
        public async void InputNotSamePasswordToRegister()
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


        /// <summary>
        /// Успешная регистрация
        /// </summary>
        [Fact]
        public async void SuccessUserRegister()
        {

            var controller = new AccountController(userManager.Object, signInManager.Object);


            RegisterUserViewModel vm = new RegisterUserViewModel()
            {
                Email = "test@email.ru",
                Password = "pass123QWE",
                PasswordConfirm = "pass123QWE",
                DateBirth = DateTime.Now
            };


            // Действие - выполняем регистрацию
            var res = await controller.Register(vm) as RedirectToActionResult;

            Assert.Equal("Index", res.ActionName);
            Assert.Equal("Home", res.ControllerName);

            // Проверяем на предмет добавления пользователя в бд
            Assert.Equal(vm.Email, _users.FirstOrDefault(i => i.Email == vm.Email).Email);
        }


        /// <summary>
        /// Проверка на ошибку состояния модели ModelState
        /// </summary>
        [Fact]
        public async void NullViewModelSendToRegister()
        {
            var controller = new AccountController(userManager.Object, signInManager.Object);
            // new code added -->
            controller.ModelState.AddModelError("fakeError", "fakeError");

            RegisterUserViewModel vm = null;


            // Действие - выполняем регистрацию
            var res = await controller.Register(vm) as ViewResult;


            Assert.Null(res.Model);
        }


        /// <summary>
        /// Неудачная регистрация
        /// </summary>
        [Fact]
        public async void FailedRegister()
        {
            // Устанавливаем метод для регистрации
            userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(new IdentityError() { Code = "FakeBadRegister", Description = "bad user register" }));

            var controller = new AccountController(userManager.Object, signInManager.Object);


            RegisterUserViewModel vm = new RegisterUserViewModel()
            {
                Email = "test@email",
                Password = "pass123QWE",
                PasswordConfirm = "pass123QWE",
                DateBirth = DateTime.Now
            };


            // Действие - выполняем регистрацию
            var res = await controller.Register(vm) as ViewResult;
            // Получаем количество фейковых ошибок
            int counts = res.ViewData.ModelState.Count;



            Assert.Equal(1, counts);
        }


        #endregion






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
