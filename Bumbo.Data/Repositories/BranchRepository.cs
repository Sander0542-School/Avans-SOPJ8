using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;

namespace Bumbo.Data.Repositories
{
    public class BranchRepository : RepositoryBase<Branch>
    {
        protected BranchRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}