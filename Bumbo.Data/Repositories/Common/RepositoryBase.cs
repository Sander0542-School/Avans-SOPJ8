using Bumbo.Data.Models.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bumbo.Data.Repositories.Common
{
    public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, ApplicationDbContext> 
        where TEntity : class, IEntity
    {
        public RepositoryBase(ApplicationDbContext context) : base(context)
        {
        }
    }

    public abstract class RepositoryBase<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        protected readonly TContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public RepositoryBase(TContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        protected virtual IQueryable<TEntity> GetQueryBase()
        {
            return DbSet;
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }

        public async Task<TEntity> Remove(params Expression<Func<TEntity, bool>>[] predicates)
        {
            var entity = await Get(predicates);
            if (entity == null) return null;

            return await Remove(entity);
        }

        public async Task<TEntity> Remove(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            return entity;
        }

        public async Task<TEntity> Get(params Expression<Func<TEntity, bool>>[] predicates)
        {
            return await predicates
                .Aggregate(
                    GetQueryBase(),
                    (query, predicate) => query.Where(predicate))
                .FirstOrDefaultAsync();
        }

        public async Task<List<TEntity>> GetAll(params Expression<Func<TEntity, bool>>[] predicates)
        {
            return await predicates
                .Aggregate(
                    GetQueryBase(),
                    (query, predicate) => query.Where(predicate))
                .ToListAsync();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            
            return entity;
        }
    }
}