using System.Linq;
using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;
namespace Bumbo.Data.Repositories
{
    public class FurloughRepository : RepositoryBase<Furlough>
    {
        public FurloughRepository(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<Furlough> GetQueryBase()
        {
            return base.GetQueryBase()
                .Include(f => f.User);
        }
    }
}
