using AutoFixture;
using BookMarket.Controllers;
using BookMarket.Models.UsersIdentity;
using BookMarket.Models.ViewModels.UsersIdentity;
using BookMarketUnitTests.Repository;
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
              new User() { Name = "testUser", Id = "123", Email = "testUser@mail.ru" }
        };

        #endregion


        public AccountControllerTests()
        {

            userManager = FakeUserManager.MockUserManager<User>(_users);
            signInManager = new Mock<FakeSignInManager>(userManager.Object);

        }

        #region Logout method


        /// <summary>
        /// Выход пользователя из аккаунта
        /// </summary>
        [Fact]
        public async void logout()
        {

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "Администратор"),
                new Claim("custom-claim", "example claim value")
            }, "mock"));


            AccountController controller = null;
            

            // Устанавливаем значение успешной авторизации, в случае, если аккаунт есть в фейковой бд
            signInManager.Setup(
                    x => x.SignOutAsync()
                ).Callback(() => {
                    controller.ControllerContext.HttpContext.User = null;
                });

            controller = new AccountController(userManager.Object, signInManager.Object);

            // Привязываем пользователя к контексту
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };


            // Действие - выходим из аккаунта
            var result = await controller.Logout() as RedirectToActionResult;


            // Проверяем на то, вышел ли из акаунта
            Assert.False(controller.User.Identity.IsAuthenticated);

            // Проверяем переход на главную страницу контроллера Home
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
        }

        #endregion


        #region post method Login


        /// <summary>
        /// Отправление пустой модели в авторизацию
        /// </summary>
        [Fact]
        public async void NullViewModelTransferToLogin()
        {
            var controller = new AccountController(userManager.Object, signInManager.Object);
            // new code added -->
            controller.ModelState.AddModelError("fakeError", "fakeError");

            LoginUserViewModel vm = null;


            // Действие - выполняем регистрацию
            var res = await controller.Login(vm) as ViewResult;


            Assert.Null(res.Model);
        }

        /// <summary>
        /// Неудачная авторизация с выводом информирующей ошибки
        /// </summary>
        [Theory]
        [InlineData("testUser123@mail.ru", "controller/action")]
        public async void BadAuthorizationUser(string email, string url)
        {

            // Устанавливаем значение успешной авторизации, в случае, если аккаунт есть в фейковой бд
            signInManager.Setup(
                    x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            var controller = new AccountController(userManager.Object, signInManager.Object);

            LoginUserViewModel vm = new LoginUserViewModel()
            {
                Email = email,
                Password = "pass123QWE",
                RememberMe = true,
                ReturnUrl = url
            };


            // Результат контроллера
            var result = await controller.Login(vm) as ViewResult;

            var error = result.ViewData.ModelState.FindKeysWithPrefix("Ошибка авторизации!").FirstOrDefault();




            Assert.NotNull(error.Value);
            Assert.Equal("Ошибка авторизации!", error.Key);
        }


        /// <summary>
        /// Успешная авторизация с переходом на URL с которого зашел на страницу авторизации
        /// </summary>
        [Theory]
        [InlineData("testUser@mail.ru", "controller/action")]
        public async void SuccesAuthorizationUserAndGoToUrl(string email, string url)
        {
            // Устанавливаем значение успешной авторизации, в случае, если аккаунт есть в фейковой бд
            signInManager.Setup(
                    x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var controller = new AccountController(userManager.Object, signInManager.Object);

            LoginUserViewModel vm = new LoginUserViewModel()
            {
                Email = email,
                Password = "pass123QWE",
                RememberMe = true,
                ReturnUrl = url
            };


            // Результат контроллера
            var result = await controller.Login(vm) as RedirectResult;

            Assert.NotNull(result.Url);
        }


        /// <summary>
        /// Успешная авторизация с переходом на главную страницу
        /// </summary>
        [Theory]
        [InlineData("testUser@mail.ru", null)]
        public async void SuccessAuthorizationUserAndGoIndexPage(string email, string url)
        {
            // Устанавливаем значение успешной авторизации, в случае если аккаунт есть в фейковой бд
            signInManager.Setup(
                    x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var controller = new AccountController(userManager.Object, signInManager.Object);

            LoginUserViewModel vm = new LoginUserViewModel()
            {
                Email = email,
                Password = "pass123QWE",
                RememberMe = true,
                ReturnUrl = url
            };


            // Результат контроллера
            var result = await controller.Login(vm) as RedirectToActionResult;

            // Проверяем успешную авторизацию с переходом на главную страницу
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);

        }

        #endregion

        #region get method Login


        /// <summary>
        /// Проверка перехода на страницу логина, если пользователь авторизован
        /// </summary>
        [Fact]
        public void UserIsAuthLoginAction()
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

            var result = controller.Login() as RedirectToActionResult;


            // Устанавливаем фейкового авторзиованного пользователя



            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
        }


        /// <summary>
        /// Проверка возвращаемости страницы авторизации, если пользователь не авторизован
        /// </summary>
        [Fact]
        public void NotAuthUserLoginAction()
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
            var result = controller.Login() as IActionResult;


            // Возвращаемый тип
            Assert.IsType<ViewResult>(result);

        }

        #endregion

        #region get method Register


        /// <summary>
        /// Проверка результата страницы, если пользователь не авторизован
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


    }
}
