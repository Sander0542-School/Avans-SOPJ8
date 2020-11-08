using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;

namespace Bumbo.Data.Repositories
{
    public class IdentityUserRepository : RepositoryBase<IdentityUser>
    {
        protected IdentityUserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}