using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bumbo.Web.Models;

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
            var dictionary = Request.Form.ToDictionary(input => input.Value.ToString());
            List<String> startTimes = dictionary.ElementAt(0).Key.Split(',').ToList();
            List<String> endTimes = dictionary.ElementAt(1).Key.Split(',').ToList();

            for (int i = 0; i < startTimes.Count; i++)
            {
                Console.WriteLine("Shift: " + startTimes[i] + " - " + endTimes[i] + " on " + (i + 1));
            }

            // _wrapper.UserAdditionalWork.Add(entity);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}