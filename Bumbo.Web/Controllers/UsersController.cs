using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Bumbo.Web.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Bumbo.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly RepositoryWrapper _wrapper;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        //private readonly RepositoryWrapper _wrapper;

        public UsersController(RepositoryWrapper wrapper, UserManager<User> usermanager, IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _userManager = usermanager;
            _wrapper = wrapper;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = await _wrapper.User.GetAll();

            return View(users);
        }
        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model)
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
                if (result.Succeeded)
                {
                    
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { area = "Identity", code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        model.Email,
                        "Reset Password",
                        $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    return RedirectToAction("Index");

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View();

            //await _wrapper.User.Add(user);
            //return RedirectToAction(nameof(Index));


        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id, string status)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _wrapper.User.Get(user => user.Id == id);
            List<SelectListItem> branchesList = await GetBranchList();

           

            EditViewModel userModel = CreateUserModel(user, branchesList);

            if (status != null)
            {
                userModel.StatusMessage = status;

            }

            if (user == null)
            {
                return NotFound();
            }
            return View(userModel);
        }



        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditViewModel model)
        {

            var user = await _wrapper.User.Get(user => user.Id == id);
            List<SelectListItem> branchesList = await GetBranchList();
            model.UserBranches = user.Branches;
            model.Contracts = user.Contracts;
            model.Branches = branchesList;

            if (ModelState.IsValid)
            {
                if (id != model.Id)
                {
                    return NotFound();
                }


                var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                if (model.PhoneNumber != phoneNumber)
                {
                    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                    if (!setPhoneResult.Succeeded)
                    {
                        model.StatusMessage = "Unexpected error when trying to set phone number.";
                        return View(model);
                    }
                }

                // Update it with the values from the view model
                user.FirstName = model.FirstName;
                user.MiddleName = model.MiddleName;
                user.LastName = model.LastName;
                user.ZipCode = model.ZipCode;
                user.Email = model.Email;
                //user.PhoneNumber = model.PhoneNumber;
                user.HouseNumber = model.HouseNumber;

                // Apply the changes if any to the db
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    EditViewModel userModel = CreateUserModel(user, branchesList);
                    return View(userModel);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            

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
        public async Task<IActionResult> EditDepartment(int? id, EditViewModel model)
        {

            var branch = model.InputBranchDepartment.BranchId;

            //var branch = model.InputBranchDepartment.Branch;
            var department = model.InputBranchDepartment.Department;

            if (id == null)
            {
                
                return NotFound();
            }
            // Get the existing student from the db
            var user = await _wrapper.User.Get(user => user.Id == id);
            List<SelectListItem> branchesList = await GetBranchList();

            EditViewModel userModel = CreateUserModel(user, branchesList);

           
            UserBranch userBranch2 = user.Branches.Where(b => b.BranchId == branch).Where(b => b.Department == department).FirstOrDefault();

            if (userBranch2 != null)
            {
                if (userBranch2.Department == department)
                {
                    string statusMessage = "Error employee of this branch already works in this department";
                    

                    return RedirectToAction("Edit", new { id = userModel.Id, status = statusMessage });
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

           

            //return View("Edit", userModel);

            return RedirectToAction("Edit", new{userModel.Id});


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
            EditViewModel userModel = CreateUserModel(user, branchesList);

            
            return RedirectToAction("Edit", new { userModel.Id });
        }

        // GET: Users/Create/Contract/1
        public async Task<IActionResult> CreateContractAsync(int id)
        {
            User user = await _wrapper.User.Get(u => u.Id == id);
            var viewModel = new ContractViewModel
            {
                UserId = user.Id,


            };

            return View(viewModel);
        }

        // POST: Users/Create/Contract
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContract(ContractViewModel model)
        {

            User user = await _wrapper.User.Get(u => u.Id == model.UserId);

            if (ModelState.IsValid)
            {
                var contract = new UserContract
                {
                    UserId = model.UserId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Function = model.Function,
                    Scale = model.Scale,
                    User = model.User


                };
                user.Contracts.Add(contract);
                var result = await _wrapper.User.Update(user);
                //var result = await _userManager.UpdateAsync(user);
                //if (result.Succeeded)
                //{

                   

                    return RedirectToAction("Edit", new { user.Id });

                //}

                //foreach (var error in result.Errors)
                //{
                //    ModelState.AddModelError(string.Empty, error.Description);
                //}
            }

            return View();

            //await _wrapper.User.Add(user);
            //return RedirectToAction(nameof(Index));


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

        static EditViewModel CreateUserModel(User user, List<SelectListItem> branchesList)
        {
            return new EditViewModel
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
