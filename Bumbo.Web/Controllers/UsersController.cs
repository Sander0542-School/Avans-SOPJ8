using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Bumbo.Web.Models.Users.DetailsViewModel;

namespace Bumbo.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly RepositoryWrapper _wrapper;
        private readonly UserManager<User> _userManager;
        //private readonly RepositoryWrapper _wrapper;

        public UsersController(RepositoryWrapper wrapper, UserManager<User> usermanager)
        {
            _userManager = usermanager;
            _wrapper = wrapper;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = await _wrapper.User.GetAll();


            return View(users);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _wrapper.User.Get(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = new User
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Birthday = model.Birthday,
                    ZipCode = model.ZipCode,
                    HouseNumber = model.HouseNumber,
                    UserName = model.Email

                };
                var result = await _userManager.CreateAsync(user);
                //await _wrapper.User.Add(user);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _wrapper.User.Get(user => user.Id == id);
            List<SelectListItem> branchesList = await GetBranchList();

            var UserDetail = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                ZipCode = user.ZipCode,
                HouseNumber = user.HouseNumber,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,

                UserBranches = user.Branches,
                Contracts = user.Contracts,
                Branches = branchesList


            };
            if (user == null)
            {
                return NotFound();
            }
            return View(UserDetail);
        }

       

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditUserViewModel model)
        {

            if (id != model.Id)
            {
                return NotFound();
            }

            // Get the existing student from the db
            var user =  await _wrapper.User.Get(user => user.Id == id);

            // Update it with the values from the view model
            user.FirstName = model.FirstName;
            user.MiddleName = model.MiddleName;
            user.LastName = model.LastName;
            user.ZipCode = model.ZipCode;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.HouseNumber = model.HouseNumber;

            // Apply the changes if any to the db
            await _userManager.UpdateAsync(user);


            //TODO fix dit
            model.UserBranches = user.Branches;
            model.Contracts = user.Contracts;



            return View(model);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _wrapper.User.Get(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _wrapper.User.Get(user => user.Id == id);
            await _wrapper.User.Remove(user);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UserExistsAsync(int id)
        {
            var user = await _wrapper.User.Get(user => user.Id == id);

            if (user == null)
            {
                return false;
            }
            else
            {
                return true;

            }
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDepartment(int id, Department department, int branch)
        {
            


            // Get the existing student from the db
            var user = await _wrapper.User.Get(user => user.Id == id);


            UserBranch userBranch2 = user.Branches.Where(b => b.BranchId == branch).Where(b => b.Department == department).FirstOrDefault();

            if (userBranch2 != null)
            {
                if (userBranch2.Department == department)
                {
                    return NotFound();
                }
            }


            var userBranch = new UserBranch
            {
                BranchId = branch,
                UserId = user.Id,
                Department = department,


            };

            user.Branches.Add(userBranch);

            await _wrapper.User.Update(user);

            List<SelectListItem> branchesList = await GetBranchList();

            EditUserViewModel userModel = CreateUserModel(user, branchesList);

            return View("Edit", userModel);

            
        }
       

        public async Task<IActionResult> DeleteBranchUser(int? id, int? branchId)
        {
            if (id == null || branchId == null)
            {
                return NotFound();
            }




            User user = await _wrapper.User.Get(m => m.Id == id);
            UserBranch userBranch = user.Branches.Where(b => b.BranchId == branchId).FirstOrDefault();



            user.Branches.Remove(userBranch);
            await _wrapper.User.Update(user);

            if (user == null)
            {
                return NotFound();
            }

            List<SelectListItem> branchesList = await GetBranchList();
            EditUserViewModel userModel = CreateUserModel(user, branchesList);

            return View("Edit", userModel);
        }

        private async Task<List<SelectListItem>> GetBranchList()
        {
            var branches = await _wrapper.Branch.GetAll();


            var branchesList = branches.Select(a =>
                                  new SelectListItem
                                  {
                                      Value = a.Id.ToString(),
                                      Text = a.Name
                                  }).ToList();
            return branchesList;
        }

        static EditUserViewModel CreateUserModel(User user, List<SelectListItem> branchesList)
        {
            return new EditUserViewModel
            {
                PhoneNumber = user.PhoneNumber,
                Id = user.Id,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Birthday = user.Birthday,
                ZipCode = user.ZipCode,
                HouseNumber = user.HouseNumber,
                Email = user.Email,

                UserBranches = user.Branches,
                Contracts = user.Contracts,
                Branches = branchesList



            };
        }

    }
}
