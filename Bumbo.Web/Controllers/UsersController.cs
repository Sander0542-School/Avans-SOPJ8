using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bumbo.Data;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Identity;
using Bumbo.Data.Models.Enums;
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
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {

                var UpdateUser = user;

                UpdateUser.UserName = user.Email;

                var result = await _userManager.CreateAsync(UpdateUser);
                //await _wrapper.User.Add(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _wrapper.User.Get(user => user.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserDetail model)
        {

            //if (id != model.Id)
            //{
            //    return NotFound();
            //}

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



            return View(user);
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
        public async Task<IActionResult> EditDepartment(int id, List<Department> Department)
        {
            //if (id != model.Id)
            //{
            //    return NotFound();
            //}


            // Get the existing student from the db
            var user = await _wrapper.User.Get(user => user.Id == id);





            return View(user);
        }
    }
}
