using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Bumbo.Data.Repositories;
using Bumbo.Web.Models.Schedule;
using Microsoft.AspNetCore.Mvc;

namespace Bumbo.Web.Controllers
{
    [Route("Branches/{branchId}/{controller}/{year}/{week}/{action=Index}")]
    public class ScheduleController : Controller
    {
        private readonly RepositoryWrapper _wrapper;

        public ScheduleController(RepositoryWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        [Route("{department}")]
        public async Task<IActionResult> Department(int branchId, int year, int week, Department department)
        {
            var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

            if (branch == null) return NotFound();

            var users = await _wrapper.User.GetUsersAndShifts(branchId, year, week, department);

            return View(new DepartmentViewModel
            {
                Year = year,
                Week = week,

                Branch = branch,

                EmployeeShifts = users.Select(user => new DepartmentViewModel.EmployeeShift
                {
                    Name = $"{user.FirstName} {(String.IsNullOrWhiteSpace(user.MiddleName) ? "" : user.MiddleName.Concat(" "))}{user.LastName}",

                    Contract = "TODO", //TODO
                    MaxHours = new TimeSpan(40, 0, 0), //TODO

                    Kpu = 12.80, //TODO

                    Shifts = user.Shifts.Select(shift => new DepartmentViewModel.Shift
                    {
                        StartTime = shift.StartTime,
                        EndTime = shift.EndTime
                    }).ToList()
                }).ToList()
            });
        }
    }
}