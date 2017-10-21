using ExamService.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.DAL.EntityMapping
{
    public class QuestionPoolMap : EntityMappingConfigurationBase<QuestionPool>
    {
        public override void Map(EntityTypeBuilder<QuestionPool> b)
        {
            //b.HasKey(x => x.Id);
            //b.Property(x => x.Question).IsRequired();
            //b.HasOne(o => o.Subcject).WithMany(m => m.QuestionPools).HasForeignKey(f => f.SubjectId);

            b.Property(c => c.Description).HasColumnType("text").HasDefaultValue(null);
            b.Property(c => c.Question).HasColumnType("text");
            b.Property(c => c.Answer).HasColumnType("text");
            b.Property(c => c.Delete).HasDefaultValue(false);
        }
    }
}
