using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Data.Repositories;
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

        public AdditionalWorkController(ILogger<AdditionalWorkController> logger, RepositoryWrapper wrapper)
        {
            _logger = logger;
            _wrapper = wrapper;
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
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var inputs = Request.Form.Where(val => val.Key != "__RequestVerificationToken").ToArray();

            var presentUserWork = await _wrapper.UserAdditionalWork.Get(workday =>
            workday.Day == model.Day, workday => workday.UserId ==
            Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            
            Console.WriteLine(presentUserWork);

            if (presentUserWork == null)
            {
                await _wrapper.UserAdditionalWork.Add(new UserAdditionalWork 
                {
                    Day = model.Day,
                    UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}