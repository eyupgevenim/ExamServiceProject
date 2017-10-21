using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Web.Backend.ViewModels.QuestionViewModels
{
    public class ListQuestionViewModel
    {
        public string Id { get; set; }
        public string Question { get; set; }
        public string Description { get; set; }
        public Option DescriptionJ { get; set; }
        public string ExamType { get; set; }
        public string ExamFormat { get; set; }//test or classic
        public string Answer { get; set; }
        public string Subject { get; set; }

        public string LessonGuid { get; set; }
        public string LessonName { get; set; }
    }
}
