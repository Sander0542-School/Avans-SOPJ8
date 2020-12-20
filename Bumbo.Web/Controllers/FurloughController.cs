using System;
using System.Linq;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Logic.Utils;
using Bumbo.Web.Models.Furlough;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Bumbo.Web.Controllers
{
    public class FurloughController : Controller
    {
        private readonly RepositoryWrapper _wrapper;
        private readonly UserManager<User> _userManager;

        private readonly IStringLocalizer<FurloughController> _localizer;

        public FurloughController(RepositoryWrapper wrapper, UserManager<User> userManager, IStringLocalizer<FurloughController> localizer)
        {
            _wrapper = wrapper;
            _userManager = userManager;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var furloughs = await _wrapper.Furlough.GetAll(f => f.UserId == int.Parse(_userManager.GetUserId(User)) && f.EndDate >= DateTime.Now);

            return View("Employee/Index", new FurloughViewModel
            {
                Furloughs = furloughs
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(int? id, FurloughViewModel.InputModel furloughModel)
        {
            if (TempData["alertMessage"] != null)
                ViewData["AlertMessage"] = TempData["alertMessage"];

            int userId = int.Parse(_userManager.GetUserId(User));

            if (ModelState.IsValid)
            {
                if (!furloughModel.IsAllDay)
                {
                    furloughModel.StartDate = furloughModel.StartDate + furloughModel.StartTime.Value;
                    furloughModel.EndDate = furloughModel.EndDate + furloughModel.EndTime.Value;
                }

                var shifts = await _wrapper.Shift.GetAll(shift => shift.UserId == userId && shift.Date > furloughModel.StartDate && shift.Date < furloughModel.EndDate);

                if (shifts.Count != 0)
                    TempData["alertMessage"] = $"{_localizer["Danger"]}:{_localizer["NotAllowed"]}";
                else
                {
                    var presentFurlough = await _wrapper.Furlough.Get(f => f.Id == id);

                    if (presentFurlough == null)
                    {
                        var furlough = new Furlough
                        {
                            UserId = userId,
                            Description = furloughModel.Description,
                            StartDate = furloughModel.StartDate,
                            EndDate = furloughModel.EndDate,
                            IsAllDay = furloughModel.IsAllDay,
                            Status = Data.Models.Enums.FurloughStatus.REQUESTED
                        };

                        if (await _wrapper.Furlough.Add(furlough) != null)
                            TempData["alertMessage"] = $"{_localizer["Success"]}:{_localizer["FurloughSaved"]}";
                    }
                    else
                    {
                        presentFurlough.Description = furloughModel.Description;
                        presentFurlough.StartDate = furloughModel.StartDate;
                        presentFurlough.EndDate = furloughModel.EndDate;
                        presentFurlough.IsAllDay = furloughModel.IsAllDay;

                        if (await _wrapper.Furlough.Update(presentFurlough) != null)
                            TempData["alertMessage"] = $"{_localizer["Success"]}:{_localizer["FurloughUpdated"]}";
                    }
                }
            }

            var furloughs = await _wrapper.Furlough.GetAll(f => f.UserId == int.Parse(_userManager.GetUserId(User)) && f.EndDate >= DateTime.Now);

            return RedirectToAction(nameof(Index), new FurloughViewModel
            {
                Furloughs = furloughs
            });
        }

        public async Task<IActionResult> Delete(int id)
        {
            int userId = int.Parse(_userManager.GetUserId(User));
            await _wrapper.Furlough.Remove(f => f.Id == id & f.UserId == userId);

            var furloughs = await _wrapper.Furlough.GetAll(f => f.UserId == int.Parse(_userManager.GetUserId(User)));

            return RedirectToAction(nameof(Index), new FurloughViewModel
            {
                Furloughs = furloughs
            });
        }

        [Route("Overview")]
        //[Authorize(Policy = "BranchManager")]
        public async Task<IActionResult> Overview()
        {
            var furloughs = await _wrapper.Furlough.GetAll(f => f.EndDate >= DateTime.Now && f.Status == Data.Models.Enums.FurloughStatus.REQUESTED);
            var users = furloughs.Select(f => f.User).Distinct().ToList();

            return View("Manager/Index", new ManagerFurloughViewModel
            {
                UserFurloughs = users.ToDictionary(user => new ManagerFurloughViewModel.User
                {
                    Id = user.Id,
                    Name = UserUtil.GetFullName(user),
                }, user => furloughs
                    .Where(f => f.User == user)
                    .Select(f => new ManagerFurloughViewModel.Furlough
                {
                    Description = f.Description,
                    StartDate = f.StartDate,
                    EndDate = f.EndDate,
                    IsAllDay = f.IsAllDay
                })
                .ToList())
            });
        }
    }
}
