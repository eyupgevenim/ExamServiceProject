using ExamService.Contracts.Repositories;
using ExamService.Contracts.UnitOfWork;
using ExamService.DAL.Data;
using ExamService.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        public UnitOfWork(DataContext context)
        {
            if (context == null)
                throw new ArgumentNullException();

            _context = context;
        }

        public T Get<T>() where T : class
        {
            return (T)Activator.CreateInstance(typeof(T), _context);
        }

        public int SaveChange()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }


        //=========================================

        public IUserRepository Users
        {
            get
            {
                return new UserRepository(_context);
            }
        }
        
        public IExamRepository Exams
        {
            get
            {
                return new ExamRepository(_context);
            }
        }

        public ILessonRepository Lessons
        {
            get
            {
                return new LessonRepository(_context);
            }
        }

        public IQuestionPoolRepository QuestionPools
        {
            get
            {
                return new QuestionPoolRepository(_context);
            }
        }

        public ISubjectRepository Subjects
        {
            get
            {
                return new SubjectRepository(_context);
            }
        }
    }
}
