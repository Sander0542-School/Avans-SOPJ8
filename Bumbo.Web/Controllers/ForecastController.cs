using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ASP;
using Microsoft.AspNetCore.Mvc;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Common;
using Bumbo.Data.Models.Enums;
using Bumbo.Logic.Forecast;
using Bumbo.Web.Models.Forecast;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Web.Controllers
{
    [Authorize(Policy = "BranchManager")]
    [Route("Branches/{branchId}/{controller}/{year?}/{weekNr?}/{department?}")]
    [Route("Branches/{branchId}/{controller}/{year?}/{weekNr?}")]
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
                weekNr = ISOWeek.GetWeekOfYear(DateTime.Now);
            }

            // Check if date is valid
            try
            {
                ISOWeek.ToDateTime(year.Value, weekNr.Value, DayOfWeek.Monday);
            }
            catch (Exception)
            {
                //return RedirectToAction("Index", new {branchId});
                return NotFound();
            }
            
            if (redirect) return RedirectToAction("Index", "Forecast", new { branchId, year, weekNr });


            // Define viewmodel variables
            viewModel.Branch = await _wrapper.Branch.Get(b => b.Id == branchId);
            if (viewModel.Branch == null) return NotFound();

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
            var data = new StockclerkViewModel()
            {
                FirstDayOfWeek = ISOWeek.ToDateTime(year, weekNr, DayOfWeek.Monday),
                BranchId = branchId,
                DaysInForecast = 7
            };
        
            return View(data);
        }

        // POST: Forecast/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create(int branchId, int year, int weekNr, [FromForm]StockclerkViewModel stockclerkViewModel)
        {
            if (!ModelState.IsValid) return RedirectToAction();

            var forecastStandards =
                _wrapper.ForecastStandard.GetAll(
                    f => f.BranchForecastStandards.All(bf => bf.BranchId != branchId)
                ).Result.ToList<IForecastStandard>();

            forecastStandards.AddRange(await _wrapper.BranchForecastStandard.GetAll(
                bf => bf.BranchId == branchId
            ));
            
            
            var forecastLogic = new ForecastLogic(forecastStandards);

            for (var i = 0; i < stockclerkViewModel.ExpectedNumberOfColi.Count; i++)
            {
                var forecast = new Forecast();
                forecast.BranchId = branchId;
                forecast.Date = ISOWeek.ToDateTime(year, weekNr, DayOfWeek.Monday).AddDays(i);

                forecast.Department = Department.KAS;
                forecast.WorkingHours = forecastLogic.GetWorkHoursCashRegister(forecast.Date);
                await _wrapper.Forecast.Add(forecast);

                forecast.Department = Department.VER;
                forecast.WorkingHours = forecastLogic.GetWorkHoursFresh(forecast.Date);
                await _wrapper.Forecast.Add(forecast);

                forecast.Department = Department.VAK;
                forecast.WorkingHours = forecastLogic.GetWorkHoursStockClerk(stockclerkViewModel.MetersOfShelves[i], stockclerkViewModel.ExpectedNumberOfColi[i]);
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

        public async Task<IActionResult> ChangeNorms(int branchId)
        {
            var data = new ChangeNormsViewModel{ BranchId = branchId };

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeNormsAsync(ChangeNormsViewModel data)
        {
            BranchForecastStandard entity = new BranchForecastStandard();

            entity.Activity = ForecastActivity.CASHIER;
            entity.Value = data.CashierValue;
            await _wrapper.BranchForecastStandard.Update(entity);

            entity.Activity = ForecastActivity.FACE_SHELVES;
            entity.Value = data.FaceShelvesValue;
            await _wrapper.BranchForecastStandard.Update(entity);

            entity.Activity = ForecastActivity.PRODUCE_DEPARTMENT;
            entity.Value = data.ProduceDepartmentValue;
            await _wrapper.BranchForecastStandard.Update(entity);

            entity.Activity = ForecastActivity.STOCK_SHELVES;
            entity.Value = data.StockShelvesValue;
            await _wrapper.BranchForecastStandard.Update(entity);

            entity.Activity = ForecastActivity.UNLOAD_COLI;
            entity.Value = data.UnloadColiValue;
            await _wrapper.BranchForecastStandard.Update(entity);

            return RedirectToAction("Index");
        }
    }
}
