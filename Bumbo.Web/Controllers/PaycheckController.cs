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

        public PayCheckController(RepositoryWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        // GET: PayCheck
        public async Task<IActionResult> Index(int branchId, int year, int monthNr)
        {
            var viewModel = new PaycheckViewModel();

            DateTime lastDay = new DateTime(year, monthNr, 1).AddDays(-1);
            DateTime firstDay = new DateTime(year, monthNr, 1).AddMonths(-1);

            var allWorkedShifts = await _wrapper.WorkedShift.GetAll(
                ws => ws.Shift.BranchId == branchId,
                ws => ws.Shift.Date <= lastDay,
                ws => ws.Shift.Date >= firstDay);
            
            Dictionary<User, List<WorkedShift>> monthlyWorkedShiftsPerUser = new Dictionary<User, List<WorkedShift>>();
            
            foreach (var workShift in allWorkedShifts)
            {
                if (monthlyWorkedShiftsPerUser.ContainsKey(workShift.Shift.User))
                {
                    if (monthlyWorkedShiftsPerUser.TryGetValue(workShift.Shift.User,out var monthlyWorkedShifts))
                    {
                        monthlyWorkedShifts.Add(workShift);
                    }
                }
                else
                {
                    monthlyWorkedShiftsPerUser.Add(workShift.Shift.User, new List<WorkedShift> {workShift});
                }
            }

            viewModel.GenerateWeeklyWorkedWorkedHoursPerUser();

            return View(await _wrapper.User.GetAll());
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
