using ExamService.DAL.EntityMapping;
using ExamService.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.DAL.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// You can either pass the Name of a connection string from web config or explicity declare one
        /// </summary>
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ef identity tables renamed
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            
            new QuestionPoolMap().Map(builder);
            new LessonMap().Map(builder);
            new SubjectMap().Map(builder);
            new ExamMap().Map(builder);


            //builder.Entity<QuestionPool>().Property(c => c.Description).HasColumnType("text").HasDefaultValue(null);
            //builder.Entity<QuestionPool>().Property(c => c.Question).HasColumnType("text");
            //builder.Entity<QuestionPool>().Property(c => c.Answer).HasColumnType("text");
            //builder.Entity<QuestionPool>().Property(c => c.Delete).HasDefaultValue(false);

            //builder.Entity<Lesson>().Property(c => c.Delete).HasDefaultValue(false);
            //builder.Entity<Subject>().Property(c => c.Delete).HasDefaultValue(false);
            //builder.Entity<Exam>().Property(c => c.Questions).HasColumnType("text");
        }

        /// <summary>
        /// All persistent entities must be declared
        /// </summary>
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<QuestionPool> QuestionPools { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
    }
}
