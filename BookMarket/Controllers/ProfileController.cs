using BookMarket.Models.UsersIdentity;
using BookMarket.Models.ViewModels.Profile;
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

        public ProfileController(IProfileService profileService)
        {
            this.profileService = profileService;
        }
        

        [HttpGet]
        public IActionResult Index(string name)
        {            
            return View("Index", name);
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

            return RedirectToAction("Index");
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


            return Redirect($"~/Profile/{name}");
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
                        return Redirect($"~/Profile/{vm.email}");
                }
            }

            return View();
        }
    }
}
