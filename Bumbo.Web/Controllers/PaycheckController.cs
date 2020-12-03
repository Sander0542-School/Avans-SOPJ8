using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Logic.PayCheck;
using Bumbo.Web.Models.Paycheck;

namespace Bumbo.Web.Controllers
{
    //  [Authorize(Policy = "BranchManager")]
    public class PayCheckController : Controller
    {
        private readonly RepositoryWrapper _wrapper;
        private readonly PaycheckViewModel _viewModel;

        public PayCheckController(RepositoryWrapper wrapper)
        {
            _wrapper = wrapper;
            _viewModel = new PaycheckViewModel();
        }

        //Get /Branches/1/PayCheck/2020/10
        [Route("Branches/{branchId}/{controller}/{year?}/{monthNr?}")]
        public async Task<IActionResult> Index(int branchId, int? year, int? monthNr)
        {
            // Check for default values
            var redirect = false;

            if (!year.HasValue)
            {
                redirect = true;
                year = DateTime.Now.Year;
            }

            if (!monthNr.HasValue)
            {
                redirect = true;
                monthNr = DateTime.Now.Month;
            }

            _viewModel.Year = year.Value;
            _viewModel.MonthNr = monthNr.Value;

            DateTime lastDay = new DateTime(year.Value, monthNr.Value, 1).AddDays(-1);
            DateTime firstDay = new DateTime(year.Value, monthNr.Value, 1).AddMonths(-1);

            if (redirect) return RedirectToAction("Index", "PayCheck", new { branchId, year, monthNr });

            for (int i = 0; firstDay.AddDays(i) <= lastDay; i += 7)
            {
                _viewModel.WeekNumbers.Add(ISOWeek.GetWeekOfYear(firstDay.AddDays(i)));
            }

            var allWorkedShifts = await _wrapper.WorkedShift.GetAll(
                ws => ws.Shift.BranchId == branchId,
                ws => ws.Shift.Date <= lastDay,
                ws => ws.Shift.Date >= firstDay);
            
            foreach (var workShift in allWorkedShifts)
            {

                SalaryBenefitViewModel vm = new SalaryBenefitViewModel
                {
                    Shift = workShift.Shift,
                    StartTime = workShift.StartTime,
                    EndTime = workShift.EndTime,
                    IsApprovedForPaycheck = workShift.IsApprovedForPaycheck,
                    Sick = workShift.Sick,
                    ShiftId = workShift.ShiftId
                };

                vm.ExtraTime += workShift.Shift.StartTime.Subtract(workShift.StartTime).TotalHours;
                vm.ExtraTime += workShift.Shift.EndTime.Subtract((TimeSpan)workShift.EndTime).TotalHours;

                if (_viewModel.MonthlyWorkedShiftsPerUser.ContainsKey(workShift.Shift.User))
                {
                    if (_viewModel.MonthlyWorkedShiftsPerUser.TryGetValue(workShift.Shift.User,out var monthlyWorkedShifts))
                    {
                        monthlyWorkedShifts.Add(vm);
                    }
                }
                else
                {
                    _viewModel.MonthlyWorkedShiftsPerUser.Add(workShift.Shift.User, new List<SalaryBenefitViewModel> {vm});
                }
            }

            _viewModel.GenerateWeeklyWorkedHoursPerUser();

            return View(_viewModel);
        }

        // GET: /Branches/1/PayCheck/2020/10/1
        //TODO: Fix routing
        [Route("details/{branchId:int}/{id:int}")]
        public async Task<IActionResult> Details(int? id, int branchId, int? year, int? monthNr)
        {
            if (id == null)
            {
                return NotFound();
            }

            _viewModel.SelectedUser = await _wrapper.User.Get(U => U.Id == id);

            if (_viewModel.SelectedUser == null)
            {
                return NotFound();
            }

            _viewModel.ScheduledShiftsPerUser = await _wrapper.Shift.GetAll(s => s.User.Id == id);

            _viewModel.MonthlyWorkedShiftsPerUser.TryGetValue(_viewModel.SelectedUser,  out var workedShifts);

            if (workedShifts == null)
            {
                return NotFound();
            }

            _viewModel.SelectedUserWorkedShifts = workedShifts;
            _viewModel.SortSelectedUserWorkedShiftsByDate();
            _viewModel.CalculateTotalDifferencePerWeek();

            return View(_viewModel);
        }

        [HttpPost]
        [Route("approve")]
        //[Authorize(Policy = "BranchManager")]
        public async Task<IActionResult> ApproveWorkhoursOverview(int branchId, int monthNr, int year)
        {
            var branch = await _wrapper.Branch.Get(b => b.Id == branchId);

            if (branch == null) return NotFound();



            //if (ModelState.IsValid)
            //{
            //    foreach (var kvp in _viewModel.MonthlyWorkedShiftsPerUser)
            //    {
            //        for (int i = 0; i < kvp.Value.Count; i++)
            //        {
            //            kvp.Value[i].IsApprovedForPaycheck = true;
            //        }
            //    }

            //    approvePaycheckViewModel.OverviewApproved = true;
            //}

            return RedirectToAction(nameof(Index), new
            {
                branchId,
                year = year,
                monthNr = monthNr
            });
        }

        //TODO: Fix routing
        [Route("SalaryBenefit/{branchId:int}/{id:int}")]
        public async Task<IActionResult> SalaryBenefit(int? id, int branchId, int? year, int? monthNr)
        {
            SalaryBenefitViewModel viewModel = new SalaryBenefitViewModel();
            PayCheckLogic pcl = new PayCheckLogic();

            DateTime lastDay = new DateTime(year.Value, monthNr.Value, 1).AddDays(-1);
            DateTime firstDay = new DateTime(year.Value, monthNr.Value, 1).AddMonths(-1);

            var allWorkedShifts = await _wrapper.WorkedShift.GetAll(
                ws => ws.Shift.BranchId == branchId,
                ws => ws.Shift.Date <= lastDay,
                ws => ws.Shift.Date >= firstDay);


            foreach (var workedShift in allWorkedShifts)
            {
                if (viewModel.PayChecks.ContainsKey(workedShift.Shift.User))
                {
                    viewModel.PayChecks.TryGetValue(workedShift.Shift.User, out var temp);
                    temp.AddPayCheck(pcl.CalculateBonus(workedShift));
                    viewModel.PayChecks.Add(workedShift.Shift.User, temp);
                }
                else
                {
                    viewModel.PayChecks.Add(workedShift.Shift.User,pcl.CalculateBonus(workedShift));
                }
            }
           
            return View(viewModel);
        }
    }
}
