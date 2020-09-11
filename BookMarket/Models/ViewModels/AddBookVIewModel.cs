using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookMarket.Models.ViewModels
{
    public class AddBookViewModel
    {

        [Required(ErrorMessage = "Пожалуйста, введите название книги")]
        [Display(Name = "Название книги")]
        public string NameBook { get; set; }


        [Required(ErrorMessage = "Пожалуйста, загрузите постер книги")]        
        [Display(Name = "Постер книги")]
        public IFormFile PosterBook { get; set; }

        [Required(ErrorMessage = "Пожалуйста, выберите автора")]
        [Display(Name = "Автор книги")]
        public int? idAuthor { get; set; }

        [Required(ErrorMessage = "Пожалуйста, загрузите содержимое книги в XML")]
        [Display(Name = "Содержимое книги (XML)")]
        public IFormFile XMLBook { get; set; }
    }
}
