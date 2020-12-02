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
    [Route("Branches/{branchId}/{controller}/{year?}/{monthNr?}")]
    public class PayCheckController : Controller
    {
        private readonly RepositoryWrapper _wrapper;
        private PaycheckViewModel _viewModel;

        public PayCheckController(RepositoryWrapper wrapper)
        {
            _wrapper = wrapper;
            _viewModel = new PaycheckViewModel();
        }

        [Route("")]
        // GET: PayCheck
        public async Task<IActionResult> Index(int branchId, int year = 2020, int monthNr = 10)
        {
            _viewModel.Year = year;
            _viewModel.MonthNr = monthNr;

            DateTime lastDay = new DateTime(year, monthNr, 1).AddDays(-1);
            DateTime firstDay = new DateTime(year, monthNr, 1).AddMonths(-1);

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

        [Route("details")]
        // GET: PayCheck/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(_viewModel.SelectedUser);
        }

        //// GET: PayCheck/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _wrapper.User.Get(U => U.Id == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        //// GET: PayCheck/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _wrapper.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(user);
        //}

        //// POST: PayCheck/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("FirstName,MiddleName,LastName,Birthday,ZipCode,HouseNumber,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _wrapper.Update(user);
        //            await _wrapper.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserExists(user.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(user);
        //}
    }
}
