using ExamService.Contracts.Repositories;
using ExamService.DAL.Data;
using ExamService.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.DAL.Repository
{
    public class UserRepository : RepositoryBase<string, ApplicationUser>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
            if (context == null)
                throw new ArgumentNullException();
        }

        public override void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public override ApplicationUser GetById(string id)
        {
            throw new NotImplementedException();
        }

        public override ApplicationUser GetFullObject(string id)
        {
            throw new NotImplementedException();
        }
    }
}
