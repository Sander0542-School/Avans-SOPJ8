using System;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bumbo.Web.Models.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Bumbo.Web.Controllers
{
    [Authorize(Policy = "YoungerThan18")]
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
            var additionalWorks = await _wrapper.UserAdditionalWork.GetAll(entity => entity.UserId == int.Parse(_userManager.GetUserId(User)));
            return View(new UserAdditionalWorkViewModel()
            {
                Schedule = additionalWorks
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserAdditionalWorkViewModel model)
        {
            if (ModelState.IsValid) {
                int userId = int.Parse(_userManager.GetUserId(User));

                var presentUserWork = await _wrapper.UserAdditionalWork.Get(workday =>
                workday.Day == model.Work.Day, workday => workday.UserId == userId);

                if (presentUserWork == null)
                {
                    await _wrapper.UserAdditionalWork.Add(new UserAdditionalWork
                    {
                        Day = model.Work.Day,
                        UserId = userId,
                        StartTime = model.Work.StartTime,
                        EndTime = model.Work.EndTime,
                    });
                }
                else
                {
                    presentUserWork.StartTime = model.Work.StartTime;
                    presentUserWork.EndTime = model.Work.EndTime;

                    await _wrapper.UserAdditionalWork.Update(presentUserWork);
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(DayOfWeek day)
        {
            int userId = int.Parse(_userManager.GetUserId(User));
            await _wrapper.UserAdditionalWork.Remove(work => work.Day == day, work => work.UserId == userId);

            return RedirectToAction("Index");
        }
    }
}