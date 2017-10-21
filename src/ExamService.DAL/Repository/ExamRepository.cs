using ExamService.Contracts.Repositories;
using ExamService.DAL.Data;
using ExamService.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.DAL.Repository
{
    public class ExamRepository : RepositoryBase<string, Exam>, IExamRepository
    {
        public ExamRepository(DataContext context) : base(context)
        {
            if (context == null)
                throw new ArgumentNullException();
        }

        public override void Delete(string id)
        {
            base.Delete(context.Exams.FirstOrDefault(x => x.Id == id));
            base.SaveChanges();
        }

        public override Exam GetById(string id)
        {
            return context.Exams.Single(s => s.Id == id);
        }

        public override Exam GetFullObject(string id)
        {
            throw new NotImplementedException();
        }
    }
}
