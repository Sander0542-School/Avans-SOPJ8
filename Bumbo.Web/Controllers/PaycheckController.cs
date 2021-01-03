using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Logic.EmployeeRules;
using Bumbo.Logic.PayCheck;
using Bumbo.Logic.Utils;
using Bumbo.Web.Models.Paycheck;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Bumbo.Web.Controllers
{
    [Authorize(Policy = "BranchManager")]
    [Route("Branches/{branchId}/{controller}/{action=Index}")]
    public class PayCheckController : Controller
    {
        private readonly RepositoryWrapper _wrapper;

        private readonly IStringLocalizer<PayCheckController> _localizer;

        public PayCheckController(RepositoryWrapper wrapper, IStringLocalizer<PayCheckController> localizer)
        {
            _wrapper = wrapper;
            _localizer = localizer;
        }

        [Route("{year?}/{month?}")]
        public async Task<IActionResult> Index(int branchId, int? year, int? month)
        {
            if (!year.HasValue || !month.HasValue)
            {
                return RedirectToAction(nameof(Index), new
                {
                    branchId,
                    year = year ?? DateTime.Today.Year,
                    month = month ?? DateTime.Today.Month,
                });
            }

            var branch = await _wrapper.Branch.Get(branch => branch.Id == branchId);

            if (branch == null) return NotFound();

            var firstDay = new DateTime(year.Value, month.Value, 1);
            var lastDay = new DateTime(year.Value, month.Value, DateTime.DaysInMonth(year.Value, month.Value));

            var workedShifts = await GetWorkedShifts(branch.Id, firstDay, lastDay);

            var users = workedShifts.Select(ws => ws.Shift.User).Distinct().ToList();

            var weekNumbers = WeekNumberBetweenDates(firstDay, lastDay);

            return View(new PaycheckViewModel
            {
                Branch = branch,
                Year = year.Value,
                Month = month.Value,
                WeekNumbers = weekNumbers,
                MonthShifts = users.ToDictionary(user => new PaycheckViewModel.User
                {
                    Id = user.Id,
                    Name = UserUtil.GetFullName(user),
                    Scale = user.Contracts?.Where(c => c.StartDate < firstDay).FirstOrDefault(c => c.EndDate >= firstDay)?.Scale ?? 0,
                    Function = user.Contracts?.Where(c => c.StartDate < firstDay).FirstOrDefault(c => c.EndDate >= firstDay)?.Function ?? "",
                }, user => workedShifts
                    .Where(ws => ws.Shift.User == user)
                    .Select(ws => new PaycheckViewModel.Shift
                    {
                        Day = ws.Shift.Date.DayOfWeek,
                        StartTime = ws.StartTime,
                        EndTime = ws.EndTime.Value,
                        Week = ISOWeek.GetWeekOfYear(ws.Shift.Date)
                    })
                    .ToList())
            });
        }

        [Route("{year}/{month}/{userId}")]
        public async Task<IActionResult> Details(int branchId, int year, int month, int userId)
        {
            var firstDay = new DateTime(year, month, 1);
            var lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var allWorkedShifts = await _wrapper.WorkedShift.GetAll(
                ws => ws.Shift.Schedule.BranchId == branchId,
                ws => ws.Shift.Date >= firstDay,
                ws => ws.Shift.Date < lastDay.AddDays(1),
                ws => ws.Shift.UserId == userId);

            var weeknumbers = WeekNumberBetweenDates(firstDay, lastDay);

            return View(new DetailsViewModel
            {
                WeekShifts = weeknumbers
                    .ToDictionary(weekNr => weekNr, weekNr => allWorkedShifts
                        .Where(workedShift => ISOWeek.GetWeekOfYear(workedShift.Shift.Date) == weekNr)
                        .Select(workedShift =>
                        {
                            var totalWorkTime = workedShift.EndTime.Value.Subtract(workedShift.StartTime);
                            var actualWorkedTime = totalWorkTime.Subtract(BreakDuration.GetDuration(totalWorkTime));

                            var totalWorkTimeShift = workedShift.Shift.EndTime.Subtract(workedShift.Shift.StartTime);
                            var actualWorkedTimeShift = totalWorkTimeShift.Subtract(BreakDuration.GetDuration(totalWorkTimeShift));

                            return new DetailsViewModel.Shift
                            {
                                ShiftId = workedShift.ShiftId,
                                StartTime = workedShift.StartTime,
                                EndTime = workedShift.EndTime.Value,
                                Day = workedShift.Shift.Date.DayOfWeek,
                                Difference = actualWorkedTime.Subtract(actualWorkedTimeShift)
                            };
                        })
                        .ToList()
                    ),
                Input = new DetailsViewModel.InputModel
                {
                    UserId = userId,
                    Year = year,
                    Month = month
                }
            });
        }

        [HttpPost, ActionName("Approve")]
        public async Task<IActionResult> ApproveMonth(int branchId, PaycheckViewModel model)
        {
            var branch = await _wrapper.Branch.Get(b => b.Id == branchId);

            if (branch == null) return NotFound();

            var alertMessage = $"{_localizer["Danger"]}:{_localizer["MessageWorkShiftsNotApproved"]}";

            if (ModelState.IsValid)
            {
                var workedShifts = (await GetWorkedShifts(branch.Id, model.Year, model.Month)).Select(shift =>
                {
                    shift.IsApprovedForPaycheck = true;

                    return shift;
                }).ToArray();

                if (_wrapper.WorkedShift.Update(workedShifts) != null)
                {
                    alertMessage = $"{_localizer["Success"]}:{_localizer["MessageWorkShiftsApproved"]}";
                }
            }

            TempData["AlertMessage"] = alertMessage;

            return RedirectToAction(nameof(SalaryBenefit), new
            {
                branchId,
                year = model.Year,
                month = model.Month
            });
        }

        [Route("{year}/{month}")]
        public async Task<IActionResult> SalaryBenefit(int branchId, int year, int month)
        {
            var viewModel = new SalaryBenefitViewModel();
            var pcl = new PayCheckLogic();

            var workedShifts = await GetWorkedShifts(branchId, year, month);

            foreach (var workedShift in workedShifts)
            {
                if (viewModel.PayChecks.ContainsKey(workedShift.Shift.User))
                {
                    viewModel.PayChecks.TryGetValue(workedShift.Shift.User, out var temp);
                    temp.AddPayCheck(pcl.CalculateBonus(workedShift));
                    viewModel.PayChecks[workedShift.Shift.User] = temp;
                }
                else
                {
                    viewModel.PayChecks.Add(workedShift.Shift.User, pcl.CalculateBonus(workedShift));
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("SavePaycheck")]
        public async Task<IActionResult> SavePaycheck(int branchId, DetailsViewModel.InputModel paycheckModel)
        {
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null) return NotFound();

            var alertMessage = $"{_localizer["Danger"]}:{_localizer["MessagePaycheckNotSaved"]}";

            if (ModelState.IsValid)
            {
                var shift = await _wrapper.WorkedShift.Get(shift1 => shift1.ShiftId == paycheckModel.ShiftId);

                if (shift != null)
                {
                    shift.StartTime = paycheckModel.StartTime;
                    shift.EndTime = paycheckModel.EndTime;

                    if ((await _wrapper.WorkedShift.Update(shift)) != null)
                    {
                        alertMessage = $"{_localizer["Success"]}:{_localizer["MessagePaycheckSaved"]}";
                    }
                }
            }

            TempData["alertMessage"] = alertMessage;

            return RedirectToAction(nameof(Details), new
            {
                userId = paycheckModel.UserId,
                branchId,
                year = paycheckModel.Year,
                month = paycheckModel.Month
            });
        }

        private async Task<List<WorkedShift>> GetWorkedShifts(int branchId, int year, int month)
        {
            var firstDay = new DateTime(year, month, 1);
            var lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            return await GetWorkedShifts(branchId, firstDay, lastDay);
        }

        private async Task<List<WorkedShift>> GetWorkedShifts(int branchId, DateTime firstDay, DateTime lastDay)
        {
            return await _wrapper.WorkedShift.GetAll(
                ws => ws.Shift.Schedule.BranchId == branchId,
                ws => ws.Shift.Date >= firstDay,
                ws => ws.Shift.Date < lastDay.AddDays(1));
        }

        private static List<int> WeekNumberBetweenDates(DateTime startDate, DateTime endDate)
        {
            var weekNumbers = new List<int>();

            for (var date = startDate; date.Date <= endDate.Date; date = date.AddDays(7))
            {
                weekNumbers.Add(ISOWeek.GetWeekOfYear(date));
            }

            return weekNumbers;
        }
    }
}
