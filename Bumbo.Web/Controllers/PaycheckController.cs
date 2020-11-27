using System;
using System.Linq;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models.Enums;
using Bumbo.Logic.EmployeeRules;
using Bumbo.Logic.Utils;
using Bumbo.Web.Models.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Bumbo.Web.Controllers
{
    //[Authorize(Policy = "BranchManager")]
    public class PaycheckController : Controller
    {
        private readonly RepositoryWrapper _wrapper;
        private readonly IStringLocalizer<ScheduleController> _localizer;

        public PaycheckController(RepositoryWrapper wrapper, IStringLocalizer<ScheduleController> localizer)
        {
            _wrapper = wrapper;
            _localizer = localizer;
        }
        
        public async Task<IActionResult> Index(int branchId, int year, int week, Department department)
        {

            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null) return NotFound();

            if (TempData["alertMessage"] != null)
            {
                ViewData["AlertMessage"] = TempData["alertMessage"];
            }

            try
            {
                var users = await _wrapper.User.GetUsersAndShifts(branch, year, week, department);

                return View(new DepartmentViewModel
                {
                    Department = department,

                    Branch = branch,

                    ScheduleApproved = branch.WeekSchedules
                        .Where(schedule => schedule.Year == year)
                        .Where(schedule => schedule.Week == week)
                        .FirstOrDefault(schedule => schedule.Department == department)?.Confirmed ?? false,

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
                                Date = shift.Date,
                                StartTime = shift.StartTime,
                                EndTime = shift.EndTime,
                                Notifications = notifications.First(pair => pair.Key.Id == shift.Id).Value
                            };
                        }).ToList()
                    }).ToList(),
                });
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
            }
        }
    }
}