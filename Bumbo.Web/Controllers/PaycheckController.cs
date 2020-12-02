using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bumbo.Data;
using Bumbo.Data.Models;
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
                if (_viewModel.MonthlyWorkedShiftsPerUser.ContainsKey(workShift.Shift.User))
                {
                    if (_viewModel.MonthlyWorkedShiftsPerUser.TryGetValue(workShift.Shift.User,out var monthlyWorkedShifts))
                    {
                        monthlyWorkedShifts.Add(workShift);
                    }
                }
                else
                {
                    _viewModel.MonthlyWorkedShiftsPerUser.Add(workShift.Shift.User, new List<WorkedShift> {workShift});
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

            _viewModel.MonthlyWorkedShiftsPerUser.TryGetValue(_viewModel.SelectedUser,  out var workedShifts);

            if (workedShifts != null)
            {
                _viewModel.SelectedUserWorkedShifts = workedShifts;
                _viewModel.SortSelectedUserWorkedShiftsByDate();
            }

            return View(_viewModel);
        }

        [HttpPost]
        [Route("details")]
        // GET: PayCheck/Details/5
        public async Task<IActionResult> Details(PaycheckViewModel viewModel)
        {

            return RedirectToAction("Details");
        }
    }
}
