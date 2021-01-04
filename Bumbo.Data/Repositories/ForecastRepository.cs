using System.Linq;
using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Repositories
{
    public class ForecastRepository : RepositoryBase<Forecast>
    {
        public ForecastRepository(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<Forecast> GetQueryBase()
        {
            return base.GetQueryBase()
                .Include(f => f.Branch);
        }
    }
}
