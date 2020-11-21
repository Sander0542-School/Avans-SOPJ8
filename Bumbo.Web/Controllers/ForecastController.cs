using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Bumbo.Logic.Forecast;
using Bumbo.Web.Models.Forecast;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Web.Controllers
{
    [Route("branches/{branchId}/forecasts/{year?}/{weekNr?}/{department?}")]
    [Route("branches/{branchId}/forecasts/{year?}/{weekNr?}")]
    public class ForecastController : Controller
    {
        private readonly RepositoryWrapper _wrapper;

        public ForecastController(RepositoryWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        /// <summary>
        /// Creates the default view for viewing forecasts for 1 week
        /// </summary>
        /// <param name="branchId">Id of the branch</param>
        /// <param name="department">Filter on department</param>
        /// <param name="year">The year of the forecast</param>
        /// <param name="weekNr">The year's week for the forecast</param>
        /// <returns>View with <see cref="ForecastViewModel"/> as parameter</returns>
        [Route("")]
        public async Task<IActionResult> Index(int branchId, Department? department, int? year, int? weekNr)
        {
            var viewModel = new ForecastViewModel();

            // Check for default values
            var redirect = false;
            if (!year.HasValue)
            {
                redirect = true;
                year = DateTime.Now.Year;
            }

            if (!weekNr.HasValue)
            {
                redirect = true;
                weekNr = DateLogic.GetWeekNumber(DateTime.Now);
            }

            // Check if date is valid
            try
            {
                DateLogic.DateFromWeekNumber(year.Value, weekNr.Value);
            }
            catch (Exception)
            {
                //return RedirectToAction("Index", new {branchId});
                return NotFound();
            }
            
            if (redirect) return RedirectToAction("Index", "Forecast", new { branchId, year, weekNr });


            // Define viewmodel variables
            viewModel.Branch = await _wrapper.Branch.Get(b => b.Id == branchId);
            viewModel.Department = department;

            var firstDayOfWeek = ISOWeek.ToDateTime(year.Value, weekNr.Value, DayOfWeek.Monday);

            viewModel.Forecasts = await _wrapper.Forecast.GetAll(
                f => f.BranchId == branchId,
                f => f.Department == department || department == null,
                // Week filter
                f => f.Date >= firstDayOfWeek,
                f => f.Date < firstDayOfWeek.AddDays(7)
            );

            viewModel.WeekNr = weekNr.Value;
            viewModel.Year = year.Value;

            return View(viewModel);
        }

        // GET: Forecast/Create
        [Route("create")]
        public async Task<IActionResult> Create(int branchId, int year, int weekNr)
        {
            var data = new StockclerkViewModel
            {
                FirstDayOfWeek = DateLogic.DateFromWeekNumber(year, weekNr),
                BranchId = branchId
            };

            return View(data);
        }

        // POST: Forecast/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create(int branchId, int year, int weekNr, [FromForm]StockclerkViewModel stockClerkViewModel)
        {
            if (!ModelState.IsValid) return RedirectToAction();

            var forecastLogic = new ForecastLogic(await _wrapper.BranchForecastStandard.GetAll(f => f.Branch.Id == branchId));

            for (var i = 0; i < stockClerkViewModel.DaysInForecast; i++)
            {
                var forecast = new Forecast();
                forecast.BranchId = branchId;
                forecast.Date = DateLogic.DateFromWeekNumber(year, weekNr).AddDays(i);

                forecast.Department = Department.KAS;
                forecast.WorkingHours = forecastLogic.GetWorkHoursCashRegister(forecast.Date);
                await _wrapper.Forecast.Add(forecast);

                forecast.Department = Department.VER;
                forecast.WorkingHours = forecastLogic.GetWorkHoursFresh(forecast.Date);
                await _wrapper.Forecast.Add(forecast);

                forecast.Department = Department.VAK;
                forecast.WorkingHours = forecastLogic.GetWorkHoursStockClerk(stockClerkViewModel.ForecastInputs[i].MetersOfShelves, stockClerkViewModel.ForecastInputs[i].ExpectedNumberOfColi);
                await _wrapper.Forecast.Add(forecast);
            }

            return RedirectToAction("Index",
            new {
                branchId,
                year,
                weekNr
            });
        }

        [Route("{dayOfWeek}/edit")]
        public virtual async  Task<IActionResult> Edit(int branchId, Department? department, int year, int weekNr, DayOfWeek dayOfWeek)
        {
            if (department == null) return NotFound();

            var date = ISOWeek.ToDateTime(year, weekNr, dayOfWeek);

            var model = await _wrapper.Forecast.Get(
                f => f.Department == department,
                f => f.Date == date,
                f => f.BranchId == branchId
            );

            if (model == null) return NotFound();

            return View(model);
        }

        [Route("{dayOfWeek}/edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Edit(int branchId, Department department, int year, int weekNr, DayOfWeek dayOfWeek, [FromForm] decimal WorkingHours)
        {
            var date = ISOWeek.ToDateTime(year, weekNr, dayOfWeek);

            var model = new Forecast()
            {
                BranchId = branchId,
                Department = department,
                Date = date,
                WorkingHours = WorkingHours,
            };

            try
            {
                await _wrapper.Forecast.Update(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _wrapper.Forecast.Get(f => f.Equals(model)) == null) return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index),
            new {
                branchId,
                year,
                weekNr,
                department
            });
        }
    }
}
