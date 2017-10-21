using ExamService.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Contracts.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        T Get<T>() where T : class;
        int SaveChange();

        //===========================

        IUserRepository Users { get; }
        IExamRepository Exams { get; }
        ILessonRepository Lessons { get; }
        IQuestionPoolRepository QuestionPools { get; }
        ISubjectRepository Subjects { get; }



    }
}
