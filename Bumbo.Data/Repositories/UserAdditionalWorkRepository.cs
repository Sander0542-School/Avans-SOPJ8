using Bumbo.Data.Models;

namespace Bumbo.Data.Repositories
{
    public class UserAdditionalWorkRepository : RepositoryBase<UserAdditionalWork>
    {
        public UserAdditionalWorkRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
