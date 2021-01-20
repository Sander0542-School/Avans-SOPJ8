using System.Linq;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Web.Models.Availability;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bumbo.Web.Controllers
{
    public class AvailabilityController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RepositoryWrapper _wrapper;
        private readonly IStringLocalizer<AvailabilityController> _localizer;

        public AvailabilityController(RepositoryWrapper wrapper, UserManager<User> userManager, IStringLocalizer<AvailabilityController> localizer)
        {
            _wrapper = wrapper;
            _userManager = userManager;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var availabilities = await _wrapper.UserAvailability.GetAll(a => a.UserId == int.Parse(_userManager.GetUserId(User)));

            return View(new IndexViewModel
            {
                Schedule = IndexViewModel.DaysOfWeek.Select(day => new IndexViewModel.Availability
                {
                    Day = day,
                    StartTime = availabilities.FirstOrDefault(availability => availability.Day == day)?.StartTime,
                    EndTime = availabilities.FirstOrDefault(availability => availability.Day == day)?.EndTime
                }).ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(_userManager.GetUserId(User));

                if (await _wrapper.UserAvailability.Remove(availability => availability.UserId == userId) != null)
                {
                    var availabilities = model.Schedule
                        .Where(availability => availability.StartTime.HasValue && availability.EndTime.HasValue)
                        .Select(availability => new UserAvailability
                        {
                            Day = availability.Day,
                            UserId = userId,
                            StartTime = availability.StartTime.Value,
                            EndTime = availability.EndTime.Value
                        }).ToArray();

                    if (await _wrapper.UserAvailability.AddRange(availabilities) != null)
                    {
                        TempData["alertMessage"] = $"success:{_localizer["The new availability was successfully saved."]}";

                        return RedirectToAction(nameof(Index));
                    }
                }

                TempData["alertMessage"] = $"danger:{_localizer["Could not save your availability, please try again."]}";
            }

            return View(model);
        }
    }
}
