using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;

namespace Bumbo.Data.Repositories
{
    public class ClockSystemTagRepository : RepositoryBase<ClockSystemTag>
    {
        protected ClockSystemTagRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}