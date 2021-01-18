using System.Linq;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Web.Models.AdditionalWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
namespace Bumbo.Web.Controllers
{
    [Authorize(Policy = "YoungerThan18")]
    public class AdditionalWorkController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RepositoryWrapper _wrapper;
        private readonly IStringLocalizer<AdditionalWorkController> _localizer;

        public AdditionalWorkController(RepositoryWrapper wrapper, UserManager<User> userManager, IStringLocalizer<AdditionalWorkController> localizer)
        {
            _wrapper = wrapper;
            _userManager = userManager;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var additionalWorks = await _wrapper.UserAdditionalWork.GetAll(a => a.UserId == int.Parse(_userManager.GetUserId(User)));

            return View(new IndexViewModel
            {
                Schedule = IndexViewModel.DaysOfWeek.Select(day => new IndexViewModel.AdditionalWork
                {
                    Day = day,
                    StartTime = additionalWorks.FirstOrDefault(availability => availability.Day == day)?.StartTime,
                    EndTime = additionalWorks.FirstOrDefault(availability => availability.Day == day)?.EndTime
                }).ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(_userManager.GetUserId(User));

                if (await _wrapper.UserAdditionalWork.Remove(availability => availability.UserId == userId) != null)
                {
                    var additionalWorks = model.Schedule
                        .Where(availability => availability.StartTime.HasValue && availability.EndTime.HasValue)
                        .Select(availability => new UserAdditionalWork
                        {
                            Day = availability.Day,
                            UserId = userId,
                            StartTime = availability.StartTime.Value,
                            EndTime = availability.EndTime.Value
                        }).ToArray();

                    if (await _wrapper.UserAdditionalWork.AddRange(additionalWorks) != null)
                    {
                        TempData["alertMessage"] = $"success:{_localizer["The new additional work was successfully saved."]}";

                        return RedirectToAction(nameof(Index));
                    }
                }

                TempData["alertMessage"] = $"danger:{_localizer["Could not save your additional work, please try again."]}";
            }

            return View(model);
        }
    }
}
