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

        public async Task<List<User>> GetUsersAndShifts(Branch branch, int year, int week, Department department)
        {
            var startTime = ISOWeek.ToDateTime(year, week, DayOfWeek.Monday);

            return await Context.Users
                .Where(user => user.Branches.Any(userBranch => userBranch.BranchId == branch.Id))
                .Where(user => user.Branches.Any(userBranch => userBranch.Department == department))
                .Where(user => user.Contracts.Any(contract => contract.StartDate < startTime))
                .Where(user => user.Contracts.Any(contract => contract.EndDate >= startTime))
                .Include(user => user.Shifts
                    .Where(shift => shift.BranchId == branch.Id)
                    .Where(shift => shift.Department == department)
                    .Where(shift => shift.Date >= startTime)
                    .Where(shift => shift.Date < startTime.AddDays(7))
                )
                .Include(user => user.Contracts
                    .Where(contract => contract.StartDate < startTime)
                    .Where(contract => contract.EndDate >= startTime)
                )
                .Include(user => user.UserAvailabilities)
                .Include(user => user.UserAdditionalWorks)
                .AsSplitQuery()
                .ToListAsync();
        }
    }
}