using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Data.Tables
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Delete { get; set; }

        public int LessonId { get; set; }
        public virtual Lesson Lesson { get; set; }
        
        public virtual ICollection<QuestionPool> QuestionPools { get; set; }

        public Subject()
        {
            this.QuestionPools = new HashSet<QuestionPool>();
        }
    }
}
