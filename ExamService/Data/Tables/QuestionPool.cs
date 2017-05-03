using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Data.Tables
{
    public class QuestionPool
    {
        [Key]
        public string Id { get; set; }
        public string ExamType { get; set; }
        public string ExamFormat { get; set; }
        public string Question { get; set; }
        public string Description { get; set; }
        public string Answer { get; set; }
        public bool Delete { get; set; }

        public int LessonId { get; set; }
        public virtual Lesson Lesson { get; set; }

        public int SubjectId { get; set; }
        public virtual Subject Subcject { get; set; }

        public QuestionPool()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
