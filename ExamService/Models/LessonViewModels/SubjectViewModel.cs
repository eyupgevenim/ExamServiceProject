using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Models.LessonViewModels
{
    public class SubjectViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Konu Adı boş geçemezsiniz !")]
        [Display(Name = "* Konu Adı :")]
        public string SubjactName { get; set; }

        public int QuestionCount { get; set; }
        public int TestCount { get; set; }
        public int ClassicCount { get; set; }

    }
}
