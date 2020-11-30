using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bumbo.Data;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Bumbo.Web.Controllers
{
    [Authorize(Policy = "BranchEmployee")]
    [Route("Branches/{branchId?}/{action=Index}")]
    public class BranchesController : Controller
    {
        private readonly RepositoryWrapper _wrapper;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly HttpContext _httpContext;

        public BranchesController(RepositoryWrapper wrapper, RoleManager<Role> roleManager, UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _wrapper = wrapper;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContext = httpContextAccessor.HttpContext;
        }

        [Authorize(Policy = "SuperUser")]
        public async Task<IActionResult> Test()
        {
            var userId = _httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return NotFound();
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.AddToRoleAsync(user, "SuperUser");

            if (_httpContext.User.IsInRole("SuperUser"))
            {
                return Ok("Hello world");
            }

            return Json(await _userManager.GetRolesAsync(user));
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
            await _wrapper.Branch.Add(branch);
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
