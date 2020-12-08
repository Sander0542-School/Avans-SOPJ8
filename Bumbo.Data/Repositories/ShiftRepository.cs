using System.Linq;
using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Repositories
{
    public class ShiftRepository : RepositoryBase<Shift>
    {
        public ShiftRepository(ApplicationDbContext context) : base(context)
        {
        }
        protected override IQueryable<Shift> GetQueryBase()
        {
            return base.GetQueryBase()
                .Include(schedule => schedule.Schedule);
        }
    }
}
