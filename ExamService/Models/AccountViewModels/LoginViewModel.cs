using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Models.AccountViewModels
{
    public class LoginViewModel
    {
        //[RegularExpression("^[0-9]*$", ErrorMessage = "text message")]
        [Required(ErrorMessage ="Kullanıcı Adı boş geçemezsiniz.")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Kullanıcı Adı boş geçemezsiniz.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla?")]
        public bool RememberMe { get; set; }
    }
}
