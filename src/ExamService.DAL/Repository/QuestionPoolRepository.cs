using ExamService.Contracts.Repositories;
using ExamService.DAL.Data;
using ExamService.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.DAL.Repository
{
    public class QuestionPoolRepository : RepositoryBase<string, QuestionPool>, IQuestionPoolRepository
    {
        public QuestionPoolRepository(DataContext context) : base(context)
        {
            if (context == null)
                throw new ArgumentNullException();
        }

        public override void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public override QuestionPool GetById(string id)
        {
            throw new NotImplementedException();
        }

        public override QuestionPool GetFullObject(string id)
        {
            throw new NotImplementedException();
        }
    }
}
