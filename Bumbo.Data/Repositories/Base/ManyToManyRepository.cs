using System.Collections.Generic;
using System.Threading.Tasks;
using Bumbo.Data.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Repositories.Base
{
    /// <summary>
    /// Base repository for all <see cref="ManyToManyBaseEntity"/> CRUD actions.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class ManyToManyRepository<TModel> : BaseRepository<TModel> where TModel : ManyToManyBaseEntity
    {
        protected ManyToManyRepository(DbContext dbContext) : base(dbContext)
        {
        }
        public async Task<TModel> Get(int typeId1, int typeId2) => await Table.FirstOrDefaultAsync(m => m.Type1Id == typeId1 && m.Type2Id == typeId2);

        public async Task<IEnumerable<TModel>> GetAll()
        {
            return await Table
                .Include(m => m.Type1)
                .Include(m => m.Type2)
                .ToListAsync();
        }
    }
}