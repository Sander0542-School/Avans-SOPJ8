using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Bumbo.Logic.Forecast;
using Bumbo.Web.Models.Forecast;

namespace Bumbo.Web.Controllers
{
    [Route("{branchId}/{controller}")]
    public class ForecastController : Controller
    {
        private readonly RepositoryWrapper _wrapper;
        private ForecastViewModel viewModel;

        public ForecastController(RepositoryWrapper wrapper)
        {
            _wrapper = wrapper;
            viewModel = new ForecastViewModel();
        }

        // GET: Forecast
        [Route("{year}/{weekNr}")]
        [Route("")]
        public async Task<IActionResult> Index(int branchId, int year = -1, int weekNr = -1)
        {
            viewModel.Branch = await _wrapper.Branch.Get(b => b.Id == branchId);

            var redirect = false;

            if (year == -1)
            {
                redirect = true;
                year = DateTime.Now.Year;
            }

            if (weekNr == -1)
            {
                redirect = true;
                weekNr = DateLogic.GetWeekNumber(DateTime.Now);
            } else if (weekNr <= 0)
            {
                redirect = true;
                weekNr = 52;
                year -= 1;
            } else if (weekNr > 52)
            {
                redirect = true;
                weekNr = 1;
                year += 1;
            }

            if (redirect) return RedirectToAction("Index", "Forecast", new { branchId, year, weekNr });

            var requestedDate = DateLogic.DateFromWeekNumber(DateTime.Now.Year, weekNr);
            viewModel.Forecasts =  _wrapper.Forecast.GetAll(f => f.BranchId == branchId).Result
                .Where(f => DateLogic.DateIsInSameWeek(f.Date, requestedDate));

            viewModel.WeekNr = weekNr;
            viewModel.Year = year;

            return View(viewModel);
        }

        // GET: Forecast/Details/5
        [Route("{year=0}/{weekNr=0}/{department}")]
        public async Task<IActionResult> Details(int branchId, Department department, int weekNr)
        {
            var forecast = await _wrapper.Forecast.Get(m => m.BranchId == branchId && m.Department == department);
            if (forecast == null)
            {
                return NotFound();
            }

            return View(forecast);
        }

        // GET: Forecast/Create
        [Route("create")]
        public async Task<IActionResult> Create()
        {
            ViewData["BranchId"] = new SelectList(await _wrapper.Branch.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Forecast/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create([Bind("BranchId,Date,Department,WorkingHours")] Forecast forecast)
        {
            if (ModelState.IsValid)
            {
                await _wrapper.Forecast.Add(forecast);
                return RedirectToAction(nameof(Index));
            }
            //ViewData["BranchId"] = new SelectList(_wrapper.Branches, "Id", "HouseNumber", forecast.BranchId);
            return View(forecast);
        }
    }
}
