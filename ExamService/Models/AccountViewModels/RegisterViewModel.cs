using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı zorunludur !")]
        [StringLength(30, ErrorMessage = "{0} en kısa {2} en uzun {1} karakter olmalı.", MinimumLength = 3)]
        [RegularExpression("^[a-zA-Z]{1,30}[0-9a-zA-Z]*$", ErrorMessage = "{0} harf ile başlamalı")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email zorunludur !")]
        [EmailAddress(ErrorMessage ="Email adresi doğru değil.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password'u boş geçemezsiniz !")]
        [StringLength(100, ErrorMessage = "{0} en kısa {2} en uzun {1} karakter olmalı.", MinimumLength = 6)]
        [DataType(DataType.Password, ErrorMessage ="En az bir büyük ile bir küçük harf, bir rakam ve bir diğer karakterler(.,*,_,-,...) içermelidir.")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password Tekrarı")]
        [Compare("Password", ErrorMessage = "Password'un tekrarı eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }
}
