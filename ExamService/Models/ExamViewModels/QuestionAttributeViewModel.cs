using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Models.ExamViewModels
{
    public class QuestionAttributeViewModel
    {
        public string eLessonGuid { get; set; }
        public string eVisa { get; set; }
        public string eFinal { get; set; }
        public string eCompletion { get; set; }
        public string eExcuse { get; set; }
        public string eSingleLesson { get; set; }
        public int eTest { get; set; }
        public int eClassic { get; set; }
        public string eGroup { get; set; }
        public string eGroupFormat { get; set; }

        public string eSubjectItems { get; set; }
    }
}
