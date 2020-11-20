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
using Microsoft.AspNetCore.Identity;

namespace Bumbo.Web.Controllers
{
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
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit()
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == null)
            {
                return RedirectToAction("Index");
            }
            
            var inputs = Request.Form.Where(val => val.Key != "__RequestVerificationToken").ToArray();

            foreach (var item in inputs)
            {
                Console.WriteLine("Hours: " + item.Value + " on " + item.Key + " for user " +
                                  User.FindFirstValue(ClaimTypes.NameIdentifier));

                // if record with userID and Day not present
                _wrapper.UserAdditionalWork.Add(new UserAdditionalWork
                {
                    Day = Convert.ToInt32(item.Key),
                    Hours = item.Value != null ? 0 : item.Value,
                    UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier))
                });

                // else
                _wrapper.UserAdditionalWork.Update(new UserAdditionalWork
                {
                    Day = Convert.ToInt32(item.Key),
                    Hours = item.Value != null ? 0 : item.Value,
                    UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier))
                });
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