using System.Linq;
using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Repositories
{
    public class WorkedShiftRepository : RepositoryBase<WorkedShift>
    {
        public WorkedShiftRepository(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<WorkedShift> GetQueryBase()
        {
            return base.GetQueryBase()
                .Include(ws => ws.Shift)
                .ThenInclude(s => s.User)
                .ThenInclude(u => u.Branches)
                .Include(ws => ws.Shift)
                .ThenInclude(s => s.Schedule);
        }
    }
}
