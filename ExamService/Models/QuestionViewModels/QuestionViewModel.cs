using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Models.QuestionViewModels
{
    public class QuestionViewModel
    {
        public string Id { get; set; }

        [Display(Name = "* Soru :")]
        public string Question { get; set; }

        public Option DescriptionJ { get; set; }

        [Display(Name = "Soru İçerik :")]
        public string Description { get; set; }

        public string qLessonGuid { get; set; }
        public string qExamType { get; set; }
        public string qExamFormat { get; set; }//test or classic
        public int qSubjectId { get; set; }

        [Display(Name = "* Cevap :")]
        public string Answer { get; set; }
        public List<SelectListItem> AnswerTestItems { get; set; }


        public QuestionViewModel()
        {
            AnswerTestItems = new List<SelectListItem>
            {
                new SelectListItem { Value = "A", Text="A" },
                new SelectListItem { Value = "B", Text="B" },
                new SelectListItem { Value = "C", Text="C" },
                new SelectListItem { Value = "D", Text="D" }
            };
        }
        
    }

    public class Option
    {
        [Required]
        public string A { get; set; }

        [Required]
        public string B { get; set; }

        public string C { get; set; }
        public string D { get; set; }
        public string E { get; set; }
    }

}
