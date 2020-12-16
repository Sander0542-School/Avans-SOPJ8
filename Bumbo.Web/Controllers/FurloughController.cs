using System.Linq;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Web.Models.Furlough;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Bumbo.Web.Controllers
{
    //TODO: scheiden van manager en medewerker
    //[Authorize(Policy = "BranchManager")]
    //[Route("Branches/{branchId}/{controller}/{action=Index}")]
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
            var furloughs = await _wrapper.Furlough.GetAll(f => f.UserId == int.Parse(_userManager.GetUserId(User)));

            return View(new FurloughViewModel
            {
                Furloughs = furloughs
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(FurloughViewModel.InputModel furloughModel)
        {
            if (TempData["alertMessage"] != null)
                ViewData["AlertMessage"] = TempData["alertMessage"];

            int userId = int.Parse(_userManager.GetUserId(User));

            if (ModelState.IsValid)
            {
                //TODO: Build checks:
                // - Check if user has no shifts during furlough request
                // - Check if isAllDay is true => change Time
                // - Check if endDate is not smaller than startDate
                // - Set balance of to request furlough hours/days
                // - Check if requesttime is nog bigger than balance

                // var shifts = await _wrapper.Shift.GetAll(shift => shift.UserId == userId && shift.Date >= furloughModel.StartDate && shift.Date <= furloughModel.EndDate);

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

            var furloughs = await _wrapper.Furlough.GetAll(f => f.UserId == int.Parse(_userManager.GetUserId(User)));

            return RedirectToAction(nameof(Index), new FurloughViewModel
            {
                Furloughs = furloughs
            });
        }

        public IActionResult Edit()
        {
            //Wijzigen van een aanvraag
            return View();
        }
    }
}
