using ExamService.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.DAL.EntityMapping
{
    public class LessonMap : EntityMappingConfigurationBase<Lesson>
    {
        public override void Map(EntityTypeBuilder<Lesson> b)
        {
            //b.HasKey(x => x.Id);

            b.Property(c => c.Delete).HasDefaultValue(false);
        }
    }
}
