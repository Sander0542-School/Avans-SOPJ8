using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;

namespace Bumbo.Data.Repositories
{
    public class WorkedShiftRepository : RepositoryBase<WorkedShift>
    {
        public WorkedShiftRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}