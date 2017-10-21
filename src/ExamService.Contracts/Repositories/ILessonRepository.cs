using ExamService.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Contracts.Repositories
{
    public interface ILessonRepository : IRepositoryBase<int, Lesson>
    {
    }
}
