using BookMarket.Models.UsersIdentity;
using BookMarket.Models.ViewModels.UsersIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookMarket.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel vm)
        {
            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    Email = vm.Email,
                    UserName = vm.Email,
                    dateBirth = vm.DateBirth
                };

                var result = await _userManager.CreateAsync(user, vm.Password);

                // Если успешная регистрация
                if (result.Succeeded)
                {
                    // Установка куки
                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                
            }

            return View(vm);
        }




        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            string url = Request.Headers["Referer"].ToString();
            return View(new LoginUserViewModel { ReturnUrl = url });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Метод контроллера для авторизацияя
        public async Task<IActionResult> Login(LoginUserViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // Авторизация
                var result = await _signInManager.PasswordSignInAsync(vm.Email, vm.Password, vm.RememberMe, false);

                // Если успешная авторизация, то 
                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(vm.ReturnUrl))
                    {
                        return Redirect(vm.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный email и (или) пароль");
                }
            }

            return View(vm);
        }

        /// <summary>
        /// Метод для выхода из аккаунта
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
