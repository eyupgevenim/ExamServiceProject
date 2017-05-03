using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Models.LessonViewModels
{
    public class LessonViewModel
    {
        [Required(ErrorMessage = "Dersin Adı boş geçemezsiniz !")]
        [StringLength(50, ErrorMessage = "{0} en az {2} en fazla {1} karekter uzunluğunda olabilir!", MinimumLength = 3)]
        //[RegularExpression("^[a-zA-ZıİğĞüÜöÖçÇşŞ]{1,50}[0-9a-zA-ZıİğĞüÜöÖçÇşŞ]*$", ErrorMessage = "{0} harfle başlamalı")]
        [Display(Name = "* Ders Adı :")]
        public string Name { get; set; }

        [Display(Name = "Dersin Kodu :")]
        public string Code { get; set; }

        [RegularExpression("^[\\d]*$")]
        [Display(Name = "Ders Saati :")]
        public int Hours { get; set; }

        [RegularExpression("^[0-9]*$")]
        [Display(Name = "Dersin Akts'si :")]
        public int Akts { get; set; }

        public string Guid { get; set; }

        public int SumQuesion { get; set; }

        public List<SubjectViewModel> Subjects { get; set; }
    }
}
