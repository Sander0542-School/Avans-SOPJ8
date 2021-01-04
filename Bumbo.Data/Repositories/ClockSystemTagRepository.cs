using System.Linq;
using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Repositories
{
    public class ClockSystemTagRepository : RepositoryBase<ClockSystemTag>
    {
        public ClockSystemTagRepository(ApplicationDbContext context) : base(context)
        {
        }
        protected override IQueryable<ClockSystemTag> GetQueryBase()
        {
            return base.GetQueryBase()
                .Include(tag => tag.User)
                .ThenInclude(user => user.Shifts);
        }
    }
}
