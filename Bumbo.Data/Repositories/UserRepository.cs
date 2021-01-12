﻿using System;
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

        protected override IQueryable<User> GetQueryBase()
        {
            return base.GetQueryBase()
                .Include(user => user.Contracts)
                .Include(user => user.Branches);
        }

        public async Task<List<User>> GetUsersAndShifts(Branch branch, int year, int week, params Department[] departments)
        {
            var startTime = ISOWeek.ToDateTime(year, week, DayOfWeek.Monday);

            return await Context.Users
                .Where(user => user.Branches.Any(userBranch => userBranch.BranchId == branch.Id))
                .Where(user => user.Branches.Any(userBranch => departments.Contains(userBranch.Department)))
                .Where(user => user.Contracts.Any(contract => contract.StartDate < startTime))
                .Where(user => user.Contracts.Any(contract => contract.EndDate >= startTime))
                .Include(user => user.Shifts
                    .Where(shift => shift.Schedule.BranchId == branch.Id)
                    .Where(shift => shift.Date >= startTime)
                    .Where(shift => shift.Date < startTime.AddDays(7))
                )
                .ThenInclude(shift => shift.Schedule)
                .Include(user => user.Shifts
                    .Where(shift => shift.Schedule.BranchId == branch.Id)
                    .Where(shift => shift.Date >= startTime)
                    .Where(shift => shift.Date < startTime.AddDays(7))
                )
                .ThenInclude(shift => shift.WorkedShift)
                .Include(user => user.Contracts
                    .Where(contract => contract.StartDate < startTime)
                    .Where(contract => contract.EndDate >= startTime)
                )
                .Include(user => user.UserAvailabilities)
                .Include(user => user.UserAdditionalWorks)
                .Include(user => user.UserFurloughs)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<List<User>> GetFreeEmployees(int branchId, DateTime date, Department department)
        {
            return await Context.Users
                .Where(user => user.Branches.Any(b => b.BranchId == branchId && b.Department == department))// Check if user works at branch and department
                .Where(user => user.Contracts.Any(contract => contract.StartDate < date))// Check for active contract
                .Where(user => user.Contracts.Any(contract => contract.EndDate >= date))
                .Where(user => user.Shifts.All(s => s.Date != date))// Get users who do not have any shifts on that day
                .ToListAsync();
        }

        public async Task<List<User>> GetUpcomingBirthdays(IEnumerable<int> branches, int limit)
        {
            try
            {
                return await Context.Users
                    .Where(user => user.Birthday != null)
                    .Where(user => user.Branches.Any(branch => branches.Contains(branch.BranchId)))
                    .OrderBy(user => EF.Functions.DateDiffDay(DateTime.Today, user.Birthday.AddYears(EF.Functions.DateDiffYear(user.Birthday, DateTime.Today) + ((user.Birthday.Month < DateTime.Today.Month || (user.Birthday.Day <= DateTime.Today.Day && user.Birthday.Month == DateTime.Today.Month)) ? 1 : 0))))
                    .Take(limit)
                    .AsSplitQuery()
                    .ToListAsync();
            }
            catch (InvalidOperationException)
            {
                if (!Context.Database.IsSqlite()) throw;
            }
            
            return new List<User>();
        }

        public async Task<List<User>> GetSickEmployees(IEnumerable<int> branches, DateTime date)
        {
            return await Context.WorkedShifts
                .Include(workedShift => workedShift.Shift)
                .ThenInclude(shift => shift.User)
                .Where(workedShift => workedShift.Shift.Date == date)
                .Where(workedShift => workedShift.Sick)
                .Select(workedShift => workedShift.Shift.User)
                .ToListAsync();
        }
    }
}
