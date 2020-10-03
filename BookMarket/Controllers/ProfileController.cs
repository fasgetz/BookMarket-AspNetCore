using BookMarket.Models.UsersIdentity;
using BookMarket.Models.ViewModels.Profile;
using BookMarket.Services.Books;
using BookMarket.Services.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService profileService;
        private readonly IBookService bookService;

        public ProfileController(IProfileService profileService, IBookService bookService)
        {
            this.bookService = bookService;
            this.profileService = profileService;
        }
        

        [HttpGet]
        public IActionResult Index(string name)
        {            
            return View("Index", name);
        }

        [HttpGet]
        public async Task<IActionResult> AboutProfile(string name)
        {
            var user = await profileService.GetUser(name);

            IndexProfileViewModel vm = new IndexProfileViewModel()
            {
                email = name,
                user = user
            };

            return PartialView("ProfileData", vm);
        }

        [HttpGet]
        public async Task<IActionResult> LastVisitBooks(string name)
        {
            // Запрос на книги
            var query = await bookService.GetLastVisitBook(name);


            return PartialView(query);
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentaries(string name)
        {
            // Запрос на комментарии
            var query = await bookService.GetLastCommentariesUser(name);

            return PartialView("Commentaries", query);
        }



        [HttpPost]
        public async Task<IActionResult> LoadAvatar()
        {
            // Получаем файл
            var file = Request.Form.Files.FirstOrDefault();


            byte[] imageData = null;

            //считываем переданный файл в массив байтов
            using (var binaryReader = new BinaryReader(file.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)file.Length);
            }

            // Обновляем аватар
            var updated = await profileService.UpdateAvatar(User.Identity.Name, imageData);

            if (!updated)
                return NotFound();

            return Redirect($"{User.Identity.Name}");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAvatar(string name)
        {
            if (name == User.Identity.Name)
            {
                var deleted = await profileService.RemoveUserAvatar(User.Identity.Name);

                if (!deleted)
                    return null;
            }

            return Redirect($"{name}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveProfile(IndexProfileViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.email == User.Identity.Name)
                {
                    var updated = await profileService.UpdateUser(vm);

                    if (updated == true)
                        return Redirect($"{vm.email}");
                }
            }

            return View();
        }
    }
}
