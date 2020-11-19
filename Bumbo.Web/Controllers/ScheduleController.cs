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

        public async Task<IActionResult> SaveShift(int branchId, DepartmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Department", new {branchId, year = viewModel.Year, week = viewModel.Week, department = viewModel.InputShift.Department});
            }
            
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null) return NotFound();

            var shift = await _wrapper.Shift.Get(
                shift1 => shift1.BranchId == branch.Id,
                shift1 => shift1.Id == viewModel.InputShift.ShiftId);

            bool success;

            if (shift == null)
            {
                shift = new Shift
                {
                    BranchId = branch.Id,
                    UserId = viewModel.InputShift.UserId,
                    Department = viewModel.InputShift.Department,
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

            if (!success)
            {
                return RedirectToAction("Department", new {branchId, year = viewModel.Year, week = viewModel.Week, department = viewModel.InputShift.Department});
            }

            return RedirectToAction("Department", new {branchId, year = viewModel.Year, week = viewModel.Week, department = viewModel.InputShift.Department});
        }
    }
}