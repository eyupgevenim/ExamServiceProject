using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ExamService.Data.Tables;

namespace ExamService.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
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
            //builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            //builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            //ef identity tables remove
            builder.Ignore<IdentityUserLogin<string>>();
            builder.Ignore<IdentityUserToken<string>>();
            //builder.Ignore<IdentityRoleClaim<string>>();
            //builder.Ignore<IdentityUserClaim<string>>();

            //builder.Entity<QuestionPool>().Property(c => c.Description).HasColumnType("text");
            builder.Entity<QuestionPool>().Property(c => c.Description).HasColumnType("text").HasDefaultValue(null);
            builder.Entity<QuestionPool>().Property(c => c.Question).HasColumnType("text");
            builder.Entity<QuestionPool>().Property(c => c.Answer).HasColumnType("text");
            builder.Entity<QuestionPool>().Property(c => c.Delete).HasDefaultValue(false);

            builder.Entity<Lesson>().Property(c => c.Delete).HasDefaultValue(false);

            builder.Entity<Subject>().Property(c => c.Delete).HasDefaultValue(false);
            
            builder.Entity<Exam>().Property(c => c.Questions).HasColumnType("text");
            //builder.Entity<Exam>().Property(c => c.Id).HasDefaultValue(Guid.NewGuid().ToString()); // try uuid for mysql 
            //builder.Entity<Exam>().Property(c => c.CreatedDate).HasDefaultValue(DateTime.Now); // try NOW() for mysql 
        }

        // to add tables in database
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<QuestionPool> QuestionPools { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
    }
}
