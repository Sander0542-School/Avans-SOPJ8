using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bumbo.Web.Models;
using Bumbo.Web.Models.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Bumbo.Web.Controllers
{
    [Authorize]
    public class AdditionalWorkController : Controller
    {
        private readonly ILogger<AdditionalWorkController> _logger;
        private readonly RepositoryWrapper _wrapper;
        private readonly UserManager<User> _userManager;

        public AdditionalWorkController(ILogger<AdditionalWorkController> logger, RepositoryWrapper wrapper, UserManager<User> userManager)
        {
            _logger = logger;
            _wrapper = wrapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var additionalWorks = await _wrapper.UserAdditionalWork.GetAll(entity => entity.UserId == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return View(new UserAdditionalWorkViewModel()
            {
                Schedule = additionalWorks
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserAdditionalWorkViewModel.InputAdditionalWork model)
        {
            var user = _userManager.GetUserAsync(User).Id;


            var presentUserWork = await _wrapper.UserAdditionalWork.Get(workday =>
            workday.Day == model.Day, workday => workday.UserId == user);

            if (presentUserWork == null)
            {
                await _wrapper.UserAdditionalWork.Add(new UserAdditionalWork
                {
                    Day = model.Day,
                    UserId = user,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                });
            }
            else
            {
                presentUserWork.StartTime = model.StartTime;
                presentUserWork.EndTime = model.EndTime;

                bool success = await _wrapper.UserAdditionalWork.Update(presentUserWork) != null;
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(DayOfWeek day)
        {
            var user = _userManager.GetUserAsync(User).Id;
            var additionalWork = await _wrapper.UserAdditionalWork.Get(work => work.Day == day, work => work.UserId == user);
            await _wrapper.UserAdditionalWork.Remove(additionalWork);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}