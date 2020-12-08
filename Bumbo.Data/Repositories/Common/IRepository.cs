using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bumbo.Data.Models.Common;

namespace Bumbo.Data.Repositories.Common
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity> Add(TEntity entity);
        Task<TEntity> Get(params Expression<Func<TEntity, bool>>[] predicates);
        Task<List<TEntity>> GetAll(params Expression<Func<TEntity, bool>>[] predicates);
        Task<TEntity> Update(TEntity entity);
        Task<List<TEntity>> Update(params TEntity[] entities);
        Task<List<TEntity>> Remove(params Expression<Func<TEntity, bool>>[] predicates);
        Task<TEntity> Remove(TEntity model);
    }
}