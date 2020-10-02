using BookMarket.Models.ViewModels.Profile;
using BookMarket.Services.Profile;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Components
{

    /// <summary>
    /// Компонент для возвращения профиля пользователя
    /// </summary>
    public class ProfileDataViewComponent : ViewComponent
    {
        IProfileService profileService;

        public ProfileDataViewComponent(IProfileService profileService)
        {
            this.profileService = profileService;
        }


        public async Task<IViewComponentResult> InvokeAsync(string userName)
        {
            var user = await profileService.GetUser(userName);

            IndexProfileViewModel vm = new IndexProfileViewModel()
            {
                email = userName,
                user = user
            };
            
            return View("ProfileData", vm);
        }
    }
}
