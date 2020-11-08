using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;

namespace Bumbo.Data.Repositories
{
    public class ForecastStandardRepository : RepositoryBase<ForecastStandard>
    {
        protected ForecastStandardRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}