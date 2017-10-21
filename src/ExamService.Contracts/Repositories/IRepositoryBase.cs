using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExamService.Contracts.Repositories
{
    public interface IRepositoryBase<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void Delete(TKey id);
        void Delete(TEntity entity);
        void Dispose();
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAll(Func<TEntity, bool> where = null);
        TEntity GetById(TKey id);
        TEntity GetFullObject(TKey id);
        IQueryable<TEntity> GetPaged<TColumn>(int top = 20, int skip = 0, Func<TEntity, TColumn> orderBy = null, Func<TEntity, bool> where = null);
        IQueryable<TEntity> GetPaged(int top = 20, int skip = 0);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void RunCommand(string query);
        Task RunCommandAsync(string query);
        void RunCommandWithParameter(string query, params object[] parameters);
        Task RunCommandWithParameterAsync(string query, CancellationToken cancellationToken = default(CancellationToken), params object[] parameters);
    }
}
