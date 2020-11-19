using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bumbo.Data;
using Bumbo.Data.Models;

namespace Bumbo.Web.Controllers
{
    public class ForecastController : Controller
    {
        private readonly RepositoryWrapper _wrapper;

        public ForecastController(RepositoryWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        // GET: Forecast
        public async Task<IActionResult> Index()
        {
            return View(await _wrapper.Forecast.GetAll());
        }

        // GET: Forecast/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forecast = await _wrapper.Forecast.Get(m => m.BranchId == id);
            if (forecast == null)
            {
                return NotFound();
            }

            return View(forecast);
        }

        // GET: Forecast/Create
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
