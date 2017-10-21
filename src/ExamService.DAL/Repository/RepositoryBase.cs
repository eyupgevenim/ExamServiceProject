using ExamService.Contracts.Repositories;
using ExamService.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExamService.DAL.Repository
{
    public abstract class RepositoryBase<TKey, TEntity> : IRepositoryBase<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class
    {
        internal DataContext context;
        internal DbSet<TEntity> dbSet;

        public RepositoryBase(DataContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }


        //!!!!!!!!!!!!!!!!!
        public abstract void Delete(TKey id);
        public abstract TEntity GetById(TKey id);
        public abstract TEntity GetFullObject(TKey id);


        public virtual int SaveChanges()
        {
            return context.SaveChanges();
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public virtual void Delete(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);

            dbSet.Remove(entity);
        }
        
        public virtual void Dispose()
        {
            context.Dispose();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return dbSet;
        }

        public virtual IQueryable<TEntity> GetAll(Func<TEntity, bool> where)
        {
            //need to override in order to implement specific filtering.
            return dbSet.Where(where).AsQueryable();
        }
        
        public virtual IQueryable<TEntity> GetPaged<TColumn>(int top = 20, int skip = 0, Func<TEntity, TColumn> orderBy = null, Func<TEntity, bool> where = null)
        {
            //need to override in order to implement specific filtering and ordering
            return dbSet.Skip(skip).Take(top).OrderBy(orderBy).Where(where).AsQueryable();
        }

        public virtual IQueryable<TEntity> GetPaged(int top = 20, int skip = 0)
        {
            //need to override in order to implement specific filtering and ordering
            return dbSet.Skip(skip).Take(top);
        }

        public virtual void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void RunCommand(string query)
        {
            context.Database.ExecuteSqlCommand(query);
        }

        public virtual async Task RunCommandAsync(string query)
        {
            await context.Database.ExecuteSqlCommandAsync(query);
        }

        public void RunCommandWithParameter(string query, params object[] parameters)
        {
            context.Database.ExecuteSqlCommand(query, parameters);
        }

        public async Task RunCommandWithParameterAsync(string query, CancellationToken cancellationToken = default(CancellationToken), params object[] parameters)
        {
            await context.Database.ExecuteSqlCommandAsync(query, cancellationToken, parameters);
        }
    }
}
