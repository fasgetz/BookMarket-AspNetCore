using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.ViewModels
{
    public class AddCommentaryOnBookViewModel
    {
        [Required(ErrorMessage = "Пожалуйста, введите комментарий")]
        [Display(Name = "Оставить комментарий")]
        public string Commentary { get; set; }

        [Required(ErrorMessage = "Пожалуйста, выберите рейтинг")]
        [Display(Name = "Выберите рейтинг")]
        public int rating { get; set; }

        // Айди книги
        public int IdBook { get; set; }

    }
}
