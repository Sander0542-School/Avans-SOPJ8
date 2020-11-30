using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bumbo.Data;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace Bumbo.Web.Controllers
{
    [Authorize(Policy = "BranchEmployee")]
    [Route("Branches/{branchId?}/{action=Index}")]
    public class BranchesController : Controller
    {
        private readonly RepositoryWrapper _wrapper;

        public BranchesController(RepositoryWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        [Authorize(Policy = "SuperUser")]
        public async Task<IActionResult> Index()
        {
            return View(await _wrapper.Branch.GetAll());
        }

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

        [Authorize(Policy = "SuperUser")]
        public IActionResult Create()
        {
            return View();
        }
        
        [Authorize(Policy = "SuperUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ZipCode,HouseNumber,Id")] Branch branch)
        {
            if (!ModelState.IsValid) return View(branch);
            var newBranch =await _wrapper.Branch.Add(branch);
            return RedirectToAction("Index");
        }

        [Authorize(Policy = "BranchManager")]
        public async Task<IActionResult> Edit(int? branchId)
        {
            if (branchId == null) return NotFound();

            var branch = await _wrapper.Branch.Get(b => b.Id == branchId);
            if (branch == null) return NotFound();
            return View(branch);
        }
        
        [Authorize(Policy = "BranchManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int branchId, [Bind("Name,ZipCode,HouseNumber,Id")] Branch branch)
        {
            if (branchId != branch.Id) return NotFound();
            if (!ModelState.IsValid) return View(branch);
            
            try
            {
                branch = await _wrapper.Branch.Update(branch);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _wrapper.Branch.Get(b => b.Id == branch.Id) == null) return NotFound();
                throw;
            }
            return RedirectToAction(
                "Details",
                new {
                    branchId = branch.Id
                });
        }

        [Authorize(Policy = "SuperUser")]
        public async Task<IActionResult> Delete(int? branchId)
        {
            if (branchId == null) return NotFound();

            var branch = await _wrapper.Branch.Get(b => b.Id == branchId);
            if (branch == null) return NotFound();

            return View(branch);
        }

        [Authorize(Policy = "SuperUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int branchId)
        {
            var branch = await _wrapper.Branch.Get(b => b.Id == branchId);
            await _wrapper.Branch.Remove(branch);
            return RedirectToAction("Index");
        }
    }
}
