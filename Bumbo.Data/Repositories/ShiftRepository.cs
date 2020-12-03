using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;

namespace Bumbo.Data.Repositories
{
    public class ShiftRepository : RepositoryBase<Shift>
    {
        public ShiftRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
