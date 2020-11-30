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
    [Route("Branches/{branchId?}/{action=Index}")]
    public class BranchesController : Controller
    {
        private readonly RepositoryWrapper _wrapper;

        public BranchesController(RepositoryWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        // GET: Branches
        public async Task<IActionResult> Index()
        {
            return View(await _wrapper.Branch.GetAll());
        }

        // GET: Branches/Details/5
        public async Task<IActionResult> Details(int? branchId)
        {
            if (branchId == null)
            {
                return NotFound();
            }

            var branch = await _wrapper.Branch.Get(b => b.Id == branchId);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // GET: Branches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ZipCode,HouseNumber,Id")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                await _wrapper.Branch.Add(branch);
                return RedirectToAction(nameof(Index));
            }
            return View(branch);
        }

        // GET: Branches/Edit/5
        public async Task<IActionResult> Edit(int? branchId)
        {
            if (branchId == null)
            {
                return NotFound();
            }

            var branch = await _wrapper.Branch.Get(b => b.Id == branchId);
            if (branch == null)
            {
                return NotFound();
            }
            return View(branch);
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int branchId, [Bind("Name,ZipCode,HouseNumber,Id")] Branch branch)
        {
            if (branchId != branch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _wrapper.Branch.Update(branch);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchExists(branch.Id))
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
            return View(branch);
        }

        // GET: Branches/Delete/5
        public async Task<IActionResult> Delete(int? branchId)
        {
            if (branchId == null)
            {
                return NotFound();
            }

            var branch = await _wrapper.Branch.Get(b => b.Id == branchId);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int branchId)
        {
            var branch = await _wrapper.Branch.Get(b => b.Id == branchId);
            await _wrapper.Branch.Remove(branch);
            return RedirectToAction(nameof(Index));
        }

        private bool BranchExists(int id)
        {
            return _wrapper.Branch.Get(b => b.Id == id).Result != null;
        }
    }
}
