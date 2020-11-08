using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;

namespace Bumbo.Data.Repositories
{
    public class WorkedShiftRepository : RepositoryBase<WorkedShift>
    {
        protected WorkedShiftRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}