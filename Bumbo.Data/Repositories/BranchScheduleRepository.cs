using System.Linq;
using System.Threading.Tasks;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Bumbo.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Repositories
{
    public class BranchScheduleRepository : RepositoryBase<BranchSchedule>
    {
        public BranchScheduleRepository(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<BranchSchedule> GetQueryBase()
        {
            return base.GetQueryBase()
                .Include(schedule => schedule.Branch);
        }

        public async Task<BranchSchedule> GetOrCreate(int branchId, int year, int week, Department department)
        {
            var schedule = await Context.Set<BranchSchedule>()
                    .Where(branchSchedule => branchSchedule.BranchId == branchId)
                    .Where(branchSchedule => branchSchedule.Year == year)
                    .Where(branchSchedule => branchSchedule.Week == week)
                    .Where(branchSchedule => branchSchedule.Department == department)
                    .FirstOrDefaultAsync();

            if (schedule != null)
            {
                return schedule;
            }

            schedule = new BranchSchedule
            {
                BranchId = branchId,
                Year = year,
                Week = week,
                Department = department,
            };

            Context.Add(schedule);
            
            var changed = await Context.SaveChangesAsync();
            return changed > 0 ? schedule : null;
        }
    }
}