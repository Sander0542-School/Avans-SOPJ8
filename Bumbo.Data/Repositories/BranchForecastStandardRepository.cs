using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;

namespace Bumbo.Data.Repositories
{
    public class BranchForecastStandardRepository : RepositoryBase<BranchForecastStandard>
    {
        public BranchForecastStandardRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}