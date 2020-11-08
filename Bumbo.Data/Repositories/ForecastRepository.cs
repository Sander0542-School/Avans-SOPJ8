using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;

namespace Bumbo.Data.Repositories
{
    public class ForecastRepository : RepositoryBase<Forecast>
    {
        protected ForecastRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}