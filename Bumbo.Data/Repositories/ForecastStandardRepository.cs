using System.Linq;
using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;
namespace Bumbo.Data.Repositories
{
    public class ForecastStandardRepository : RepositoryBase<ForecastStandard>
    {
        public ForecastStandardRepository(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<ForecastStandard> GetQueryBase()
        {
            return base.GetQueryBase()
                .Include(f => f.BranchForecastStandards);
        }
    }
}
