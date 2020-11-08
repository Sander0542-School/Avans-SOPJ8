using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;

namespace Bumbo.Data.Repositories
{
    public class BranchForecastStandardRepository : RepositoryBase<BranchForecastStandard>
    {
        protected BranchForecastStandardRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}