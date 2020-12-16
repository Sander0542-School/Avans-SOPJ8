using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;

namespace Bumbo.Data.Repositories
{
    public class FurloughRepository : RepositoryBase<Furlough>
    {
        public FurloughRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
