using BookMarket.Controllers;
using BookMarket.Models.UsersIdentity;
using BookMarketUnitTests.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Xunit;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace BookMarketUnitTests.Controllers
{


    public class UsersControllerTests
    {
        #region Свойства

        Mock<UserManager<User>> userManager;
        Mock<FakeSignInManager> signInManager;

        /// <summary>
        /// Список пользователей юзер менеджера
        /// </summary>
        private List<User> _users = new List<User>
        {
              new User() { Name = "testUser", Id = "123", Email = "testUser@mail.ru" },
              new User() { Name = "testUser123", Id = "123", Email = "testUser123@mail.ru" }
        };

        #endregion

        public UsersControllerTests()
        {
            userManager = FakeUserManager.MockUserManager<User>(_users);
            //signInManager = new Mock<FakeSignInManager>(userManager.Object);

        }



        /// <summary>
        /// Переход на страницу Index контроллера Users, которая возвращает список пользователей
        /// </summary>
        [Fact]
        public void GoIndex()
        {
            // Arrange
            // Возвращаем список пользователей
            userManager.Setup(x => x.Users).Returns(_users.AsQueryable());
            var controller = new UsersController(userManager.Object);


            var result = controller.Index() as ViewResult;

            var data = (IEnumerable<User>)result.Model; // Список пользователей

            Assert.Equal(_users.Count, data.Count());
        }
    }
}
