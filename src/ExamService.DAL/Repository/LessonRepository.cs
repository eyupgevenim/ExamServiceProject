using ExamService.Contracts.Repositories;
using ExamService.DAL.Data;
using ExamService.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.DAL.Repository
{
    public class LessonRepository : RepositoryBase<int, Lesson>, ILessonRepository
    {
        public LessonRepository(DataContext context) : base(context)
        {
            if (context == null)
                throw new ArgumentNullException();
        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override Lesson GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override Lesson GetFullObject(int id)
        {
            throw new NotImplementedException();
        }
    }
}
