using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Models.ExamViewModels
{
    public class ExamGroupViewModel
    {
        public string Group { get; set; }
        public string GroupFormat { get; set; }
        public List<QuestionGroup> QuestionGroups { get; set; }
    }

    public class QuestionGroup
    {
        public List<TestGroup> TestGroups { get; set; }
        public List<ClassicGroup> ClassicGroups { get; set; }
    }

    public class TestGroup
    {
        public string Id { get; set; }
    }

    public class ClassicGroup
    {
        public string Id { get; set; }
    }
}
