using System.Linq;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Web.Controllers
{
    public class ForecastStandardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ForecastStandardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ForecastStandard
        public async Task<IActionResult> Index()
        {
            return View(await _context.ForecastStandard.ToListAsync());
        }

        // GET: ForecastStandard/Details/5
        public async Task<IActionResult> Details(ForecastActivity id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forecastStandard = await _context.ForecastStandard
                .FirstOrDefaultAsync(m => m.Activity == id);
            if (forecastStandard == null)
            {
                return NotFound();
            }

            return View(forecastStandard);
        }

        // GET: ForecastStandard/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ForecastStandard/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Activity,Value")] ForecastStandard forecastStandard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(forecastStandard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(forecastStandard);
        }

        // GET: ForecastStandard/Edit/5
        public async Task<IActionResult> Edit(ForecastActivity id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forecastStandard = await _context.ForecastStandard.FindAsync(id);
            if (forecastStandard == null)
            {
                return NotFound();
            }
            return View(forecastStandard);
        }

        // POST: ForecastStandard/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ForecastActivity id, [Bind("Activity,Value")] ForecastStandard forecastStandard)
        {
            if (id != forecastStandard.Activity)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(forecastStandard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ForecastStandardExists(forecastStandard.Activity))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(forecastStandard);
        }

       private bool ForecastStandardExists(ForecastActivity id)
        {
            return _context.ForecastStandard.Any(e => e.Activity == id);
        }
    }
}
