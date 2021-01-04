using System.Linq;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Web.Models.Branches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Bumbo.Web.Controllers
{
    [Authorize] // Fallback to prevent accidental unauthorized use
    [Route("Branches/{branchId}/{action=Details}")]
    [Route("Branches/{action=Index}")]
    public class BranchesController : Controller
    {
        private readonly RepositoryWrapper _wrapper;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly HttpContext _httpContext;
        private readonly IStringLocalizer<BranchesController> _localizer;

        public BranchesController(RepositoryWrapper wrapper, IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager, UserManager<User> userManager, IStringLocalizer<BranchesController> localizer)
        {
            _wrapper = wrapper;
            _localizer = localizer;
            _signInManager = signInManager;
            _userManager = userManager;
            _httpContext = httpContextAccessor.HttpContext;
        }

        [Authorize(Policy = "SuperUser")]
        public async Task<IActionResult> Index()
        {
            return View(await _wrapper.Branch.GetAll());
        }

        [Authorize(Policy = "BranchEmployee")]
        public async Task<IActionResult> Details(int branchId)
        {
            var branch = await _wrapper.Branch.Get(b => b.Id == branchId);
            if (branch == null)
            {
                return NotFound();
            }

            var managersForBranch = _wrapper.BranchManager.GetAll(bm => bm.BranchId == branchId).Result
                .Select(bm => bm.User).ToList();

            return View(new DetailsViewModel
            {
                CurrentUserId = _userManager.GetUserAsync(User).Id,
                Branch = branch,
                Managers = managersForBranch
            });
        }

        [Authorize(Policy = "SuperUser")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "SuperUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ZipCode,HouseNumber")] Branch branch)
        {
            if (!ModelState.IsValid) return View(branch);
            branch = await _wrapper.Branch.Add(branch);

            // TODO: Don't add super users to their created branches
            // This is currently done to allow super users to access created branches
            // https://stackoverflow.com/questions/65094900/net-core-super-user-policy
            var user = await _userManager.GetUserAsync(User);
            await AddManagerToBranchAsync(branch.Id, user.Id);
            await _signInManager.RefreshSignInAsync(user); // Updates user claims so it immediately has access to branch

            return RedirectToAction(
                "Details",
                new
                {
                    branchId = branch.Id
                });
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
                new
                {
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

            var allBranchUsers = _wrapper.UserBranch.GetAll(ub => ub.BranchId == branch.Id).Result.Select(ub => ub.User).ToList();
            allBranchUsers.AddRange(_wrapper.BranchManager.GetAll(bm => bm.BranchId == branch.Id).Result.Select(bm => bm.User));

            await _wrapper.Branch.Remove(branch);
            foreach (var user in allBranchUsers) await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction("Index", new { branchId = "" });
        }

        [Authorize(Policy = "BranchManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveManager(int userId, int branchId)
        {
            // Manager can't remove itself
            if (_userManager.GetUserAsync(User).Id == userId) return RedirectToAction("Details");

            var branchManager = await _wrapper.BranchManager.Get(
                bm => bm.BranchId == branchId,
                bm => bm.UserId == userId
            );
            await _wrapper.BranchManager.Remove(branchManager);

            return RedirectToAction("Details");
        }

        [Authorize(Policy = "BranchManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddManager(string userEmail, int branchId)
        {
            var user = await _wrapper.User.Get(u => u.Email == userEmail);
            if (user == null)
            {
                TempData["alertMessage"] = $"{_localizer["This user does not exist"]}";
                return RedirectToAction("Details");
            }

            // Check if manager already exists
            if (await _wrapper.BranchManager.Get(
                bm => bm.BranchId == branchId,
                bm => bm.UserId == user.Id
            ) != null)
            {
                return RedirectToAction("Details");
            }
                

            await AddManagerToBranchAsync(branchId, user.Id);

            return RedirectToAction("Details");
        }

        private async Task AddManagerToBranchAsync(int branchId, int userId)
        {
            var result = await _wrapper.BranchManager.Add(new BranchManager
            {
                BranchId = branchId,
                UserId = userId
            });
        }
    }
}
