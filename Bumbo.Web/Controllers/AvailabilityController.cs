using System;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Web.Models.Schedule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bumbo.Web.Controllers
{
    public class AvailabilityController : Controller
    {
        private readonly RepositoryWrapper _wrapper;
        private readonly UserManager<User> _userManager;

        public AvailabilityController(RepositoryWrapper wrapper, UserManager<User> userManager)
        {
            _wrapper = wrapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var availabilities = await _wrapper.UserAvailability.GetAll(a => a.UserId == int.Parse(_userManager.GetUserId(User)));
            return View(new UserAvailabilityViewModel
            {
                Schedule = availabilities
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserAvailabilityViewModel model)
        {
            if (ModelState.IsValid)
            {
                int userId = int.Parse(_userManager.GetUserId(User));

                var presentUserAvailability = await _wrapper.UserAvailability.Get(workday =>
                workday.Day == model.Availability.Day, workday => workday.UserId == userId);

                if (presentUserAvailability == null)
                {
                    await _wrapper.UserAvailability.Add(new UserAvailability
                    {
                        Day = model.Availability.Day,
                        UserId = userId,
                        StartTime = model.Availability.StartTime,
                        EndTime = model.Availability.EndTime,
                    });
                }
                else
                {
                    presentUserAvailability.StartTime = model.Availability.StartTime;
                    presentUserAvailability.EndTime = model.Availability.EndTime;

                    await _wrapper.UserAvailability.Update(presentUserAvailability);
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(DayOfWeek day)
        {
            int userId = int.Parse(_userManager.GetUserId(User));
            await _wrapper.UserAvailability.Remove(work => work.Day == day, work => work.UserId == userId);

            return RedirectToAction("Index");
        }
    }
}
