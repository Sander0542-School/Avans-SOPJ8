using System.Collections.Generic;
using System.Threading.Tasks;
using Bumbo.Data.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Repositories.Base
{
    /// <summary>
    /// Base repository for all <see cref="BaseEntity"/> CRUD actions.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class BaseEntityRepository<TModel> : BaseRepository<TModel> where TModel : BaseEntity
    {
        public BaseEntityRepository(DbContext dbContext) : base(dbContext)
        {
        }
        public async Task<TModel> Get(int id) => await Table.FindAsync(id);

        public async Task<IEnumerable<TModel>> Get(IEnumerable<int> ids)
        {
            var list = new List<TModel>();
            foreach (var id in ids)
            {
                list.Add(await Get(id));
            }
            return list;
        }
    }
}