using BookMarket.Models.UsersIdentity;
using BookMarket.Models.ViewModels.UsersIdentity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookMarket.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;


        [ActivatorUtilitiesConstructor]
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Register()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
                
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel vm)
        {

            if (ModelState.IsValid)
            {
                if (!vm.Password.Equals(vm.PasswordConfirm))
                {
                    ModelState.AddModelError("SamePassword", "Введите одинаковые пароли");

                    return View(vm);

                }


                User user = new User()
                {
                    Email = vm.Email,
                    UserName = vm.Email,
                    dateBirth = vm.DateBirth,
                    dateRegistration = DateTime.Now
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
            if (!User.Identity.IsAuthenticated)
            {
                string url = Request.Headers["Referer"].ToString();
                return View(new LoginUserViewModel { ReturnUrl = url });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

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
                    ModelState.AddModelError("Ошибка авторизации!", "Неправильный email и (или) пароль");
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
