using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Web.Backend.ViewModels.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Boş geçemezsiniz !")]
        [EmailAddress(ErrorMessage = "Doğru email'inizi girin.")]
        public string Email { get; set; }
    }
}
