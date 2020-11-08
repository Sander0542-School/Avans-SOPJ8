using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;

namespace Bumbo.Data.Repositories
{
    public class UserAdditionalWorkRepository : RepositoryBase<UserAdditionalWork>
    {
        public UserAdditionalWorkRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}