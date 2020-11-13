using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Bumbo.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<User>> GetUsersAndShifts(int branchId, int year, int week, params Department[] departments)
        {
            return await Context.Users
                .Include(user => user.Shifts
                    .Where(shift => departments.Contains(shift.Department))
                    .Where(shift => shift.StartTime >= ISOWeek.ToDateTime(year, week, DayOfWeek.Monday))
                    .Where(shift => shift.StartTime < ISOWeek.ToDateTime(year, week, DayOfWeek.Monday).AddDays(7))
                )
                .Include(user => user.UserAvailabilities)
                .Include(user => user.UserAdditionalWorks)
                // .Where(user => user.BranchId == branchId) //TODO
                .ToListAsync();
        }
    }
}