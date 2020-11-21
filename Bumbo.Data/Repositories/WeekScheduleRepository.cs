using Bumbo.Data.Models;
using Bumbo.Data.Repositories.Common;

namespace Bumbo.Data.Repositories
{
    public class WeekScheduleRepository : RepositoryBase<WeekSchedule>
    {
        public WeekScheduleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}