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

            if (TempData["alertMessage"] != null)
            {
                ViewData["AlertMessage"] = TempData["alertMessage"];
            }

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
                                Date = shift.Date,
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

        [HttpPost]
        public async Task<IActionResult> SaveShift(int branchId, DepartmentViewModel viewModel)
        {
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null) return NotFound();

            var alertMessage = "Danger:De dienst kon niet worden opgeslagen.";

            if (ModelState.IsValid)
            {
                var shift = await _wrapper.Shift.Get(
                    shift1 => shift1.BranchId == branch.Id,
                    shift1 => shift1.Id == viewModel.InputShift.ShiftId);

                bool success;

                if (shift == null)
                {
                    shift = new Shift
                    {
                        Department = viewModel.Department,
                        BranchId = branch.Id,
                        UserId = viewModel.InputShift.UserId,
                        Date = viewModel.InputShift.Date,
                        StartTime = viewModel.InputShift.StartTime,
                        EndTime = viewModel.InputShift.EndTime
                    };

                    success = await _wrapper.Shift.Add(shift) != null;
                }
                else
                {
                    shift.StartTime = viewModel.InputShift.StartTime;
                    shift.EndTime = viewModel.InputShift.EndTime;

                    success = await _wrapper.Shift.Update(shift) != null;
                }

                if (success)
                {
                    alertMessage = "Success:De dienst is succesvol opgeslagen.";
                }
            }

            TempData["alertMessage"] = alertMessage;

            return RedirectToAction(nameof(Department), new
            {
                branchId,
                year = viewModel.Year,
                week = viewModel.Week,
                department = viewModel.Department,
            });
        }

        [HttpPost]
        public async Task<IActionResult> CopySchedule(int branchId, DepartmentViewModel viewModel)
        {
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null) return NotFound();
            
            TempData["alertMessage"] = "Danger:De week kon niet worden gekopieerd";

            if (ModelState.IsValid)
            {
                try
                {
                    var startDateTarget = ISOWeek.ToDateTime(viewModel.InputCopyWeek.Year, viewModel.InputCopyWeek.Week, DayOfWeek.Monday);

                    var targetShifts = await _wrapper.Shift.GetAll(
                        shift => shift.BranchId == branch.Id,
                        shift => shift.Department == viewModel.Department,
                        shift => shift.Date >= startDateTarget,
                        shift => shift.Date < startDateTarget.AddDays(7)
                    );

                    if (!targetShifts.Any())
                    {
                        var startDate = ISOWeek.ToDateTime(viewModel.Year, viewModel.Week, DayOfWeek.Monday);
                        var shifts = await _wrapper.Shift.GetAll(
                            shift => shift.BranchId == branch.Id,
                            shift => shift.Department == viewModel.Department,
                            shift => shift.Date >= startDate,
                            shift => shift.Date < startDate.AddDays(7)
                        );

                        var newShifts = shifts.Select(shift => new Shift
                        {
                            BranchId = shift.BranchId,
                            Department = shift.Department,
                            UserId = shift.UserId,
                            Date = ISOWeek.ToDateTime(viewModel.InputCopyWeek.Year, viewModel.InputCopyWeek.Week, shift.Date.DayOfWeek),
                            StartTime = shift.StartTime,
                            EndTime = shift.EndTime
                        }).ToArray();

                        if (await _wrapper.Shift.AddRange(newShifts) != null)
                        {
                            TempData["alertMessage"] = "Success:De week is succesvol gekopieerd";

                            return RedirectToAction(nameof(Department), new
                            {
                                branchId,
                                year = viewModel.InputCopyWeek.Year,
                                week = viewModel.InputCopyWeek.Week,
                                department = viewModel.Department
                            });
                        }
                    }
                    else
                    {
                        TempData["alertMessage"] = "Danger:De gekozen week bevat al diensten.";
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    TempData["alertMessage"] = "Danger:De gekozen week bestaat niet.";
                }
            }

            return RedirectToAction(nameof(Department), new
            {
                branchId,
                year = viewModel.Year,
                week = viewModel.Week,
                department = viewModel.Department
            });
        }
    }
}