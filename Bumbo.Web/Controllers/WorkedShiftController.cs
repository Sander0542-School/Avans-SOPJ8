using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bumbo.Web.Controllers
{
    [Route("WorkedShifts/{branchId}/{controller}/{action=Week}")]
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
        public async IActionResult Week(int branchId, int? year, int? week)
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
                var departments = GetUserDepartments(User, branchId);

                var user = await _wrapper.User.GetShiftsForUser(int.Parse(_userManager.GetUserId(User)), branch, year.Value, week.Value);
            }
            catch
            {
                throw new ArgumentOutOfRangeException();
            }



            return View();
        }

        private Department[] GetUserDepartments(ClaimsPrincipal user, int branchId) => User.HasClaim("Manager", branchId.ToString()) ? Enum.GetValues<Department>() : Enum.GetValues<Department>().Where(department => user.HasClaim("BranchDepartment", $"{branchId}.{department}")).ToArray();

    }
}
