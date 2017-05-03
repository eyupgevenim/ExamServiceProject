using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Models.ExamViewModels
{
    public class ExamViewModel
    {
        public string Group { get; set; }
        public string GroupFormat { get; set; }
        public List<Test> Test { get; set; }
        public List<Classic> Classic { get; set; }
    }

    public class Test
    {
        public string Id { get; set; }
        public string Question { get; set; }
        public Option DescriptionJ { get; set; }
        public string Answer { get; set; }
    }

    public class Classic
    {
        public string Id { get; set; }
        public string Question { get; set; }
        public string Description { get; set; }
        public string Answer { get; set; }
    }

    public class Option
    {
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string E { get; set; }
    }
}
