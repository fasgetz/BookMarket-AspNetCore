using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.ViewModels.Books
{
    public class EditCommentVM
    {
        public int IdComment { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите комментарий")]
        [Display(Name = "Комментарий")]
        public int IdBook { get; set; }

        [Required(ErrorMessage = "Пожалуйста, выберите рейтинг")]
        [Display(Name = "Выберите рейтинг")]
        public int Mark { get; set; }
        public string comment { get; set; }

    }
}
