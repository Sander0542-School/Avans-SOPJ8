using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Web.Controllers
{
    [Authorize(Policy = "SuperUser")]
    public class ForecastStandardController : Controller
    {
        private readonly RepositoryWrapper _wrapper;

        public ForecastStandardController(RepositoryWrapper wrapper) => _wrapper = wrapper;

        public async Task<IActionResult> Index() => View(await _wrapper.ForecastStandard.GetAll());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Activity,Value")] ForecastStandard forecastStandard)
        {
            if (!ModelState.IsValid) return View(forecastStandard);
            await _wrapper.ForecastStandard.Add(forecastStandard);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(ForecastActivity activity)
        {
            var forecastStandard = await _wrapper.ForecastStandard.Get(fs => fs.Activity == activity);
            if (forecastStandard == null) return NotFound();
            return View(forecastStandard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ForecastActivity id, [Bind("Activity,Value")] ForecastStandard forecastStandard)
        {
            if (id != forecastStandard.Activity) return NotFound();
            if (!ModelState.IsValid) return View(forecastStandard);

            try
            {
                await _wrapper.ForecastStandard.Update(forecastStandard);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _wrapper.ForecastStandard.Get(
                    fs => fs.Activity == forecastStandard.Activity,
                    fs => fs.Value == forecastStandard.Value) == null)
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
