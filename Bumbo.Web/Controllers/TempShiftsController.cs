using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bumbo.Data;
using Bumbo.Data.Models;

namespace Bumbo.Web.Controllers
{
    public class TempShiftsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TempShiftsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TempShifts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Shifts.Include(s => s.Schedule).Include(s => s.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TempShifts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shift = await _context.Shifts
                .Include(s => s.Schedule)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shift == null)
            {
                return NotFound();
            }

            return View(shift);
        }

        // GET: TempShifts/Create
        public IActionResult Create()
        {
            ViewData["ScheduleId"] = new SelectList(_context.Set<BranchSchedule>(), "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: TempShifts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScheduleId,UserId,Date,StartTime,EndTime,Offered,Id")] Shift shift)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ScheduleId"] = new SelectList(_context.Set<BranchSchedule>(), "Id", "Id", shift.ScheduleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", shift.UserId);
            return View(shift);
        }

        // GET: TempShifts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shift = await _context.Shifts.FindAsync(id);
            if (shift == null)
            {
                return NotFound();
            }
            ViewData["ScheduleId"] = new SelectList(_context.Set<BranchSchedule>(), "Id", "Id", shift.ScheduleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", shift.UserId);
            return View(shift);
        }

        // POST: TempShifts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScheduleId,UserId,Date,StartTime,EndTime,Offered,Id")] Shift shift)
        {
            if (id != shift.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shift);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShiftExists(shift.Id))
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
            ViewData["ScheduleId"] = new SelectList(_context.Set<BranchSchedule>(), "Id", "Id", shift.ScheduleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", shift.UserId);
            return View(shift);
        }

        // GET: TempShifts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shift = await _context.Shifts
                .Include(s => s.Schedule)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shift == null)
            {
                return NotFound();
            }

            return View(shift);
        }

        // POST: TempShifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shift = await _context.Shifts.FindAsync(id);
            _context.Shifts.Remove(shift);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShiftExists(int id)
        {
            return _context.Shifts.Any(e => e.Id == id);
        }
    }
}
