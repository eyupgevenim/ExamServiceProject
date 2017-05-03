using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Data.Tables
{
    public class Lesson
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Hours { get; set; }
        public int Akts { get; set; }
        public string Guid { get; set; }
        public bool Delete { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<QuestionPool> QuestionPools { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public Lesson()
        {
            this.Guid = System.Guid.NewGuid().ToString();
            this.QuestionPools = new HashSet<QuestionPool>();
            this.Exams = new HashSet<Exam>();
            this.Subjects = new HashSet<Subject>();
        }
    }
}
