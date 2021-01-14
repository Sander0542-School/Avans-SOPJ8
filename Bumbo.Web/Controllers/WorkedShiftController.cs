using System;
using System.Globalization;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Web.Models.WorkedShifts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace Bumbo.Web.Controllers
{
    [Authorize(Policy = "BranchManager")]
    [Route("Branches/{branchId}/{controller}/{action=Week}")]
    public class WorkedShiftController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RepositoryWrapper _wrapper;

        public WorkedShiftController(RepositoryWrapper wrapper, UserManager<User> userManager)
        {
            _wrapper = wrapper;
            _userManager = userManager;
        }

        [Route("{year?}/{week?}")]
        public async Task<IActionResult> Week(int branchId, int? year, int? week)
        {
            if (!year.HasValue || !week.HasValue)
            {
                return RedirectToAction(nameof(Week), new
                {
                    branchId,
                    year = year ?? DateTime.Today.Year,
                    week = week ?? ISOWeek.GetWeekOfYear(DateTime.Today)
                });
            }

            var branch = await _wrapper.Branch.Get(b => b.Id == branchId);

            if (branch == null)
            {
                return NotFound();
            }

            try
            {
                var userId = int.Parse(_userManager.GetUserId(User));

                var date = ISOWeek.ToDateTime(year.Value, week.Value, DayOfWeek.Monday);


                return View(new WorkedShiftsViewModel
                {
                    Year = year.Value,
                    Week = week.Value,
                    Branch = branch,
                    WorkedShifts = await _wrapper.WorkedShift.GetAll(shift => shift.Shift.UserId == userId, shift => shift.Shift.Date >= date, shift => shift.Shift.Date < date.AddDays(7))
                });
            }
            catch
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}
