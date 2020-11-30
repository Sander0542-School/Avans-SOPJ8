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
    [Authorize(Policy = "BranchManager")]
    [Route("Branches/{branchId}/{controller}/{action=Index}")]
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

        [Route("{department}/{year?}/{week?}")]
        public async Task<IActionResult> Department(int branchId, Department department, int? year, int? week)
        {
            year ??= DateTime.Today.Year;
            week ??= ISOWeek.GetWeekOfYear(DateTime.Today);
            
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null) return NotFound();

            if (TempData["alertMessage"] != null)
            {
                ViewData["AlertMessage"] = TempData["alertMessage"];
            }

            try
            {
                var users = await _wrapper.User.GetUsersAndShifts(branch, year.Value, week.Value, department);

                return View(new DepartmentViewModel
                {
                    Year = year.Value,
                    Week = week.Value,

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

                        MaxHours = WorkingHours.MaxHoursPerWeek(user, year.Value, week.Value),

                        Scale = user.Contracts.FirstOrDefault()?.Scale ?? 0,

                        Shifts = user.Shifts.Select(shift =>
                        {
                            var notifications = WorkingHours.ValidateWeek(user, year.Value, week.Value);

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
        public async Task<IActionResult> SaveShift(int branchId, DepartmentViewModel.InputShiftModel shiftModel)
        {
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null) return NotFound();

            var alertMessage = $"Danger:{_localizer["ShiftNotSaved"]}";

            if (ModelState.IsValid)
            {
                var shift = await _wrapper.Shift.Get(
                    shift1 => shift1.BranchId == branch.Id,
                    shift1 => shift1.Id == shiftModel.ShiftId);

                bool success;

                if (shift == null)
                {
                    shift = new Shift
                    {
                        Department = shiftModel.Department,
                        BranchId = branch.Id,
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

            return RedirectToAction(nameof(Department), new
            {
                branchId,
                year = shiftModel.Year,
                week = shiftModel.Week,
                department = shiftModel.Department,
            });
        }

        [HttpPost]
        public async Task<IActionResult> CopySchedule(int branchId, DepartmentViewModel.InputCopyWeekModel copyWeekModel)
        {
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null) return NotFound();

            TempData["alertMessage"] = $"Danger:{_localizer["ScheduleNotSaved"]}";

            if (ModelState.IsValid)
            {
                try
                {
                    var startDateTarget = ISOWeek.ToDateTime(copyWeekModel.TargetYear, copyWeekModel.TargetWeek, DayOfWeek.Monday);

                    var targetShifts = await _wrapper.Shift.GetAll(
                        shift => shift.BranchId == branch.Id,
                        shift => shift.Department == copyWeekModel.Department,
                        shift => shift.Date >= startDateTarget,
                        shift => shift.Date < startDateTarget.AddDays(7)
                    );

                    if (!targetShifts.Any())
                    {
                        var startDate = ISOWeek.ToDateTime(copyWeekModel.Year, copyWeekModel.Week, DayOfWeek.Monday);
                        var shifts = await _wrapper.Shift.GetAll(
                            shift => shift.BranchId == branch.Id,
                            shift => shift.Department == copyWeekModel.Department,
                            shift => shift.Date >= startDate,
                            shift => shift.Date < startDate.AddDays(7)
                        );

                        var newShifts = shifts.Select(shift => new Shift
                        {
                            BranchId = shift.BranchId,
                            Department = shift.Department,
                            UserId = shift.UserId,
                            Date = ISOWeek.ToDateTime(copyWeekModel.TargetYear, copyWeekModel.TargetWeek, shift.Date.DayOfWeek),
                            StartTime = shift.StartTime,
                            EndTime = shift.EndTime
                        }).ToArray();

                        if (await _wrapper.Shift.AddRange(newShifts) != null)
                        {
                            TempData["alertMessage"] = $"Success:{_localizer["ScheduleCopied", copyWeekModel.TargetWeek, copyWeekModel.TargetYear]}";

                            return RedirectToAction(nameof(Department), new
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

            return RedirectToAction(nameof(Department), new
            {
                branchId,
                year = copyWeekModel.Year,
                week = copyWeekModel.Week,
                department = copyWeekModel.Department
            });
        }

        [HttpPost]
        public async Task<IActionResult> ApproveSchedule(int branchId, DepartmentViewModel.InputApproveScheduleModel approveScheduleModel)
        {
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null) return NotFound();

            TempData["alertMessage"] = $"Danger:{_localizer["ScheduleNotApproved"]}";

            if (ModelState.IsValid)
            {
                try
                {
                    var startDate = ISOWeek.ToDateTime(approveScheduleModel.Year, approveScheduleModel.Week, DayOfWeek.Monday);
                    var shifts = await _wrapper.Shift.GetAll(
                        shift => shift.BranchId == branch.Id,
                        shift => shift.Department == approveScheduleModel.Department,
                        shift => shift.Date >= startDate,
                        shift => shift.Date < startDate.AddDays(7)
                    );

                    if (shifts.Any())
                    {
                        var weekSchedule = new WeekSchedule
                        {
                            BranchId = branch.Id,
                            Year = approveScheduleModel.Year,
                            Week = approveScheduleModel.Week,
                            Department = approveScheduleModel.Department,
                            Confirmed = true
                        };

                        if (await _wrapper.WeekSchedule.Add(weekSchedule) != null)
                        {
                            TempData["alertMessage"] = $"Success:{_localizer["ScheduleApproved"]}";
                        }
                    }
                    else
                    {
                        TempData["alertMessage"] = $"Success:{_localizer["ScheduleEmpty"]}";
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    TempData["alertMessage"] = $"Danger:{_localizer["WeekNotExists"]}";
                }
            }

            return RedirectToAction(nameof(Department), new
            {
                branchId,
                year = approveScheduleModel.Year,
                week = approveScheduleModel.Week,
                department = approveScheduleModel.Department
            });
        }

       
    }
}