using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Bumbo.Web.Models.WorkedShifts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bumbo.Web.Controllers
{
    [Route("Branches/{branchId}/{controller}/{action=Week}")]
    public class WorkedShiftController : Controller
    {
        private readonly ILogger<WorkedShiftController> _logger;
        private readonly RepositoryWrapper _wrapper;
        private readonly UserManager<User> _userManager;

        public WorkedShiftController(ILogger<WorkedShiftController> logger, RepositoryWrapper wrapper, UserManager<User> userManager)
        {
            _logger = logger;
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
                    week = week ?? ISOWeek.GetWeekOfYear(DateTime.Today),
                });
            }

            var branch = await _wrapper.Branch.Get(b => b.Id == branchId);

            if (branch == null)
                return NotFound();

            try
            {
                int userId = int.Parse(_userManager.GetUserId(User));

                var date = ISOWeek.ToDateTime(year.Value, week.Value, DayOfWeek.Monday);


                return View(new WorkedShiftsViewModel
                {
                    Year = year.Value,
                    Week = week.Value,

                    Branch = branch,

                    WorkedShifts = await _wrapper.WorkedShift.GetAll(shift => shift.Shift.UserId == userId, shift => shift.Shift.Date >= date, shift => shift.Shift.Date < date.AddDays(7)),
                });
            }
            catch
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}
