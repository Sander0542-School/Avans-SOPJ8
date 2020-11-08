using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;

namespace Bumbo.Data.Repositories
{
    public class UserAdditionalWorkRepository : RepositoryBase<UserAdditionalWork>
    {
        protected UserAdditionalWorkRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}