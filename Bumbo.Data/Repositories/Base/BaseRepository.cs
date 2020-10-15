using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Repositories.Base
{
    /// <summary>
    /// Repository base class for only looking up items based on their model. Should not be inherited as a CRUD repository for most model,
    /// instead use <see cref="BaseEntityRepository{TModel}"/> for <see cref="Models.Common.BaseEntity"/>
    /// or <see cref="ManyToManyRepository{M}"/> for <see cref="Models.Common.ManyToManyBaseEntity"/>
    /// </summary>
    /// This generic type is based on EF's minimal requirement for a model
    public abstract class BaseRepository<TModel> where TModel : class
    {
        private readonly DbContext _dbContext;
        protected readonly DbSet<TModel> Table;

        protected BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            Table = _dbContext.Set<TModel>();
        }

        public async void Add(IEnumerable<TModel> itemList) => await Table.AddRangeAsync(itemList);
        public async void Add(TModel item) => await Table.AddAsync(item);
        public void Delete(IEnumerable<TModel> itemList) => Table.RemoveRange(itemList);
        public void Delete(TModel item) => Table.Remove(item);
        public async Task<IEnumerable<TModel>> Get() => await Table.ToListAsync();
        public void Update(TModel item) => Table.Update(item);
        public void Update(IEnumerable<TModel> itemList) => Table.UpdateRange(itemList);
        public async Task<bool> Exists(TModel item) => await Table.FirstOrDefaultAsync(i => i.Equals(item)) == null;
        public async Task Save() => await _dbContext.SaveChangesAsync();
    }
}