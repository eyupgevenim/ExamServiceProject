using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ExamService.Data.Tables
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Lesson> Lessons { get; set; }
        public ApplicationUser()
        {
            this.Lessons = new HashSet<Lesson>();
        }
    }
}
