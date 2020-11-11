using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;

namespace Bumbo.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}