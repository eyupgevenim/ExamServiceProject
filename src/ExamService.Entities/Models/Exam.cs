using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Entities.Models
{
    public class Exam
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Questions { get; set; }
        public DateTime CreatedDate { get; set; }

        public int LessonId { get; set; }
        public virtual Lesson Lesson { get; set; }

        public Exam()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedDate = DateTime.Now;
        }
    }
}
