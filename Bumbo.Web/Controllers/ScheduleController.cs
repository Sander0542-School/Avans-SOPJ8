using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Bumbo.Logic.EmployeeRules;
using Bumbo.Logic.Utils;
using Bumbo.Web.Models.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Bumbo.Web.Controllers
{
    [Authorize(Policy = "BranchEmployee")]
    [Route("Branches/{branchId}/{controller}/{action=Week}")]
    public class ScheduleController : Controller
    {
        private readonly RepositoryWrapper _wrapper;
        private readonly IStringLocalizer<ScheduleController> _localizer;
        private readonly UserManager<User> _userManager;

        public ScheduleController(RepositoryWrapper wrapper, IStringLocalizer<ScheduleController> localizer, UserManager<User> userManager)
        {
            _wrapper = wrapper;
            _localizer = localizer;
            _userManager = userManager;
        }

        [Route("{year?}/{week?}/{department?}")]
        public async Task<IActionResult> Week(int branchId, int? year, int? week, Department? department)
        {
            if (!year.HasValue || !week.HasValue)
            {
                return RedirectToAction(nameof(Week), new
                {
                    branchId,
                    year = year ?? DateTime.Today.Year,
                    week = week ?? ISOWeek.GetWeekOfYear(DateTime.Today),
                });
            }

            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null) return NotFound();

            if (TempData["alertMessage"] != null)
            {
                ViewData["AlertMessage"] = TempData["alertMessage"];
            }

            try
            {
                var departments = GetUserDepartments(User, branchId);

                if (department.HasValue)
                {
                    if (departments.Contains(department.Value))
                    {
                        departments = new[] { department.Value };
                    }
                    else
                    {
                        return RedirectToAction(nameof(Week), new
                        {
                            branchId,
                            year,
                            week
                        });
                    }
                }

                var users = await _wrapper.User.GetUsersAndShifts(branch, year.Value, week.Value, departments);

                return View(new DepartmentViewModel
                {
                    Year = year.Value,
                    Week = week.Value,

                    Department = department,

                    Branch = branch,

                    ScheduleApproved = department.HasValue && users.Any(user => user.Shifts.Any(shift => shift.Schedule.Department == department.Value && shift.Schedule.Confirmed)),

                    EmployeeShifts = users.Select(user =>
                    {
                        var notifications = WorkingHours.ValidateWeek(user, year.Value, week.Value);

                        return new DepartmentViewModel.EmployeeShift
                        {
                            UserId = user.Id,
                            Name = UserUtil.GetFullName(user),
                            Contract = user.Contracts.FirstOrDefault()?.Function ?? "",

                            MaxHours = WorkingHours.MaxHoursPerWeek(user, year.Value, week.Value),

                            Scale = user.Contracts.FirstOrDefault()?.Scale ?? 0,

                            Shifts = user.Shifts.Select(shift =>
                            {
                                return new DepartmentViewModel.Shift
                                {
                                    Id = shift.Id,
                                    Department = shift.Schedule.Department,
                                    Date = shift.Date,
                                    StartTime = shift.StartTime,
                                    EndTime = shift.EndTime,
                                    Notifications = notifications.First(pair => pair.Key.Id == shift.Id).Value
                                };
                            }).ToList()
                        };
                    }).ToList(),

                    InputShift = new DepartmentViewModel.InputShiftModel
                    {
                        Year = year.Value,
                        Week = week.Value,
                        Department = department
                    },

                    InputCopyWeek = new DepartmentViewModel.InputCopyWeekModel
                    {
                        Year = year.Value,
                        Week = week.Value,
                        Department = department
                    },

                    InputApproveSchedule = new DepartmentViewModel.InputApproveScheduleModel
                    {
                        Year = year.Value,
                        Week = week.Value,
                        Department = department
                    }
                });
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("SaveShift")]
        [Authorize(Policy = "BranchManager")]
        public async Task<IActionResult> SaveShift(int branchId, DepartmentViewModel.InputShiftModel shiftModel)
        {
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null) return NotFound();

            var alertMessage = $"Danger:{_localizer["ShiftNotSaved"]}";

            if (ModelState.IsValid)
            {
                var shift = await _wrapper.Shift.Get(shift1 => shift1.Id == shiftModel.ShiftId);

                bool success;

                if (shift == null)
                {
                    var schedule = await _wrapper.BranchSchedule.GetOrCreate(branch.Id, shiftModel.Year, shiftModel.Week, shiftModel.Department.Value);

                    shift = new Shift
                    {
                        ScheduleId = schedule.Id,
                        UserId = shiftModel.UserId,
                        Date = shiftModel.Date,
                        StartTime = shiftModel.StartTime,
                        EndTime = shiftModel.EndTime
                    };

                    success = await _wrapper.Shift.Add(shift) != null;
                }
                else
                {
                    shift.StartTime = shiftModel.StartTime;
                    shift.EndTime = shiftModel.EndTime;

                    success = await _wrapper.Shift.Update(shift) != null;
                }

                if (success)
                {
                    alertMessage = $"Success:{_localizer["ShiftSaved"]}";
                }
            }

            TempData["alertMessage"] = alertMessage;

            return RedirectToAction(nameof(Week), new
            {
                branchId,
                year = shiftModel.Year,
                week = shiftModel.Week,
                department = shiftModel.Department,
            });
        }

        [HttpPost]
        [Route("Copy")]
        [Authorize(Policy = "BranchManager")]
        public async Task<IActionResult> CopySchedule(int branchId, DepartmentViewModel.InputCopyWeekModel copyWeekModel)
        {
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null) return NotFound();

            TempData["alertMessage"] = $"Danger:{_localizer["ScheduleNotSaved"]}";

            if (ModelState.IsValid)
            {
                try
                {
                    var targetSchedule = await _wrapper.BranchSchedule.GetOrCreate(branch.Id, copyWeekModel.TargetYear, copyWeekModel.TargetWeek, copyWeekModel.Department.Value);
                    var targetShifts = await _wrapper.Shift.GetAll(shift => shift.ScheduleId == targetSchedule.Id);

                    if (!targetShifts.Any())
                    {
                        var schedule = await _wrapper.BranchSchedule.GetOrCreate(branch.Id, copyWeekModel.Year, copyWeekModel.Week, copyWeekModel.Department.Value);
                        var shifts = await _wrapper.Shift.GetAll(shift => shift.ScheduleId == schedule.Id);

                        var newShifts = shifts.Select(shift => new Shift
                        {
                            ScheduleId = targetSchedule.Id,
                            UserId = shift.UserId,
                            Date = ISOWeek.ToDateTime(copyWeekModel.TargetYear, copyWeekModel.TargetWeek, shift.Date.DayOfWeek),
                            StartTime = shift.StartTime,
                            EndTime = shift.EndTime
                        }).ToArray();

                        if (await _wrapper.Shift.AddRange(newShifts) != null)
                        {
                            TempData["alertMessage"] = $"Success:{_localizer["ScheduleCopied", copyWeekModel.TargetWeek, copyWeekModel.TargetYear]}";

                            return RedirectToAction(nameof(Week), new
                            {
                                branchId,
                                year = copyWeekModel.TargetYear,
                                week = copyWeekModel.TargetWeek,
                                department = copyWeekModel.Department
                            });
                        }
                    }
                    else
                    {
                        TempData["alertMessage"] = $"Danger:{_localizer["ScheduleNotEmpty"]}";
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    TempData["alertMessage"] = $"Danger:{_localizer["WeekNotExists"]}";
                }
            }

            return RedirectToAction(nameof(Week), new
            {
                branchId,
                year = copyWeekModel.Year,
                week = copyWeekModel.Week,
                department = copyWeekModel.Department
            });
        }

        [HttpPost]
        [Route("Approve")]
        [Authorize(Policy = "BranchManager")]
        public async Task<IActionResult> ApproveSchedule(int branchId, DepartmentViewModel.InputApproveScheduleModel approveScheduleModel)
        {
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null) return NotFound();

            TempData["alertMessage"] = $"Danger:{_localizer["ScheduleNotApproved"]}";

            if (ModelState.IsValid)
            {
                try
                {
                    var schedule = await _wrapper.BranchSchedule.GetOrCreate(branch.Id, approveScheduleModel.Year, approveScheduleModel.Week, approveScheduleModel.Department.Value);
                    var shifts = await _wrapper.Shift.GetAll(shift => shift.ScheduleId == schedule.Id);

                    if (shifts.Any())
                    {
                        schedule.Confirmed = true;

                        if (await _wrapper.BranchSchedule.Update(schedule) != null)
                        {
                            TempData["alertMessage"] = $"Success:{_localizer["ScheduleApproved"]}";
                        }
                    }
                    else
                    {
                        TempData["alertMessage"] = $"Danger:{_localizer["ScheduleEmpty"]}";
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    TempData["alertMessage"] = $"Danger:{_localizer["WeekNotExists"]}";
                }
            }

            return RedirectToAction(nameof(Week), new
            {
                branchId,
                year = approveScheduleModel.Year,
                week = approveScheduleModel.Week,
                department = approveScheduleModel.Department
            });
        }


        public IActionResult Personal()
        {
            return View(new EventViewModel());
        }

        [HttpGet]
        public async Task<JsonResult> GetCalendarEvents(int branchId, [FromQuery(Name = "start")] DateTime startDate, [FromQuery(Name = "end")] DateTime endDate)
        {
            var viewModel = new EventViewModel();
            var events = new List<EventViewModel>();

            var userId = int.Parse(_userManager.GetUserId(User));

            var shifts = await _wrapper.Shift.GetAll(
                shift => shift.UserId == userId,
                shift => shift.Schedule.BranchId == branchId,
                shift => shift.Date  >= startDate,
                shift => shift.Date <= endDate
            );
            
            foreach (var shift in shifts)
            {
                if (shift.Schedule.Confirmed)
                {
                    events.Add(new EventViewModel()
                    {
                        Id = shift.Id,
                        Title = shift.Schedule.Department.ToString(),
                        Start = $"{shift.Date:yyyy-MM-dd}T{shift.StartTime}",
                        End = $"{shift.Date:yyyy-MM-dd}T{ shift.EndTime }",
                        AllDay = false
                    });

                }
            }

            return Json(events.ToArray());
        }

        private Department[] GetUserDepartments(ClaimsPrincipal user, int branchId) => User.HasClaim("Manager", branchId.ToString()) ? Enum.GetValues<Department>() : Enum.GetValues<Department>().Where(department => user.HasClaim("BranchDepartment", $"{branchId}.{department}")).ToArray();
    }
}
