using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Models.QuestionViewModels
{
    public class QuestionSummaryViewModel
    {
        public string LessonName { get; set; }
        public string LessonGuid { get; set; }
        public List<GrupQuestion> Questions { get; set; }
    }

    public class GrupQuestion
    {
        public string ExamTypeName { get; set; }
        public int Count { get; set; }
    }
}
