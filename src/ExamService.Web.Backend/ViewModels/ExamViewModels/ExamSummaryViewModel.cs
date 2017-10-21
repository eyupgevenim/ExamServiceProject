using ExamService.Web.Backend.ViewModels.ExamViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Web.Backend.ViewModels.ExamViewModels
{
    public class ExamSummaryViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public ExamGroupViewModel Exam { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LessonName { get; set; }
    }
}
