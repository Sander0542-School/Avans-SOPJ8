using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Bumbo.Data.Repositories;
using Bumbo.Logic.EmployeeRules;
using Bumbo.Logic.Utils;
using Bumbo.Web.Models.Schedule;
using Microsoft.AspNetCore.Mvc;

namespace Bumbo.Web.Controllers
{
    [Route("Branches/{branchId}/{controller}/{action=Index}")]
    public class ScheduleController : Controller
    {
        private readonly RepositoryWrapper _wrapper;

        public ScheduleController(RepositoryWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        [Route("{year}/{week}/{department}")]
        public async Task<IActionResult> Department(int branchId, int year, int week, Department department)
        {
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null) return NotFound();

            try
            {
                var users = await _wrapper.User.GetUsersAndShifts(branch, year, week, department);

                return View(new DepartmentViewModel
                {
                    Year = year,
                    Week = week,
                    
                    Department = department,

                    Branch = branch,

                    EmployeeShifts = users.Select(user => new DepartmentViewModel.EmployeeShift
                    {
                        UserId = user.Id,
                        Name = UserUtil.GetFullName(user),
                        Contract = user.Contracts.FirstOrDefault()?.Function ?? "",

                        MaxHours = WorkingHours.MaxHoursPerWeek(user, year, week),

                        Scale = user.Contracts.FirstOrDefault()?.Scale ?? 0,

                        Shifts = user.Shifts.Select(shift =>
                        {
                            var notifications = WorkingHours.ValidateWeek(user, year, week);

                            return new DepartmentViewModel.Shift
                            {
                                Id = shift.Id,
                                StartTime = shift.StartTime,
                                EndTime = shift.EndTime,
                                Notifications = notifications.First(pair => pair.Key.Id == shift.Id).Value
                            };
                        }).ToList()
                    }).ToList()
                });
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
            }
        }
    }
}