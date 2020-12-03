using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Bumbo.Data.Repositories
{
    public class BranchManagerRepository : RepositoryBase<BranchManager>
    {
        public BranchManagerRepository(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<BranchManager> GetQueryBase()
        {
            return base.GetQueryBase()
                .Include(bm => bm.User);
        }
    }
}