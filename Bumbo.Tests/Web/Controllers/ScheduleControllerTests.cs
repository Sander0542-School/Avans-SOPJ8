using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Bumbo.Web.Controllers;
using Bumbo.Web.Models.Schedule;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace Bumbo.Tests.Web.Controllers
{
    public class ScheduleControllerTests : ControllerTestBase<ScheduleController>
    {
        private User _user;
        private Branch _branch;

        private ScheduleController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new ScheduleController(Wrapper, Localizer);
            _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _branch = Wrapper.Branch.Add(new Branch
            {
                Id = 1,
                Name = "Test Bumbo",
                ZipCode = "1234 AB",
                HouseNumber = "10a"
            }).Result;

            _user = Wrapper.User.Add(new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Birthday = DateTime.Today.AddYears(-30),
                Branches = new List<UserBranch>
                {
                    new UserBranch
                    {
                        Department = Department.VAK,
                        BranchId = _branch.Id
                    }
                },
                UserAvailabilities = new List<UserAvailability>
                {
                    new UserAvailability
                    {
                        Day = DayOfWeek.Monday,
                        StartTime = new TimeSpan(15,0,0),
                        EndTime = new TimeSpan(21,0,0)
                    }
                }
            }).Result;
        }

        [Test, Order(1)]
        public async Task TestAddShift()
        {
            var model = new DepartmentViewModel.InputShiftModel
            {
                UserId = _user.Id,
                Department = _user.Branches.First().Department,
                Year = 2020,
                Week = 50,
                StartTime = TimeSpan.FromHours(16),
                EndTime = TimeSpan.FromHours(18),
                Date = ISOWeek.ToDateTime(2020,50, DayOfWeek.Monday)
            };

            var result = await _controller.SaveShift(_branch.Id, model);
            var redirectResult = result as RedirectToActionResult;
            
            Assert.IsNotNull(redirectResult);
            Assert.IsNotNull(_controller.TempData["alertMessage"]);
            
            var expectedRedirectValues = new RouteValueDictionary
            {
                { "branchId", _branch.Id },
                { "year", model.Year },
                { "week", model.Week },
                { "department", model.Department }
            };
            
            Assert.AreEqual(nameof(ScheduleController.Week), redirectResult.ActionName);
            Assert.AreEqual(expectedRedirectValues, redirectResult.RouteValues);

            var shift = await Wrapper.Shift.Get(shift1 => shift1.UserId == _user.Id);
            
            Assert.NotNull(shift);
            Assert.AreEqual(_user.Id, shift.UserId);
        }

        [Test, Order(2)]
        public async Task TestEditShift()
        {
            var shift = await Wrapper.Shift.Get(shift1 => shift1.UserId == _user.Id);
            
            Assert.NotNull(shift);
            Assert.AreEqual(_user.Id, shift.UserId);
            
            var model = new DepartmentViewModel.InputShiftModel
            {
                ShiftId = shift.Id,
                UserId = _user.Id,
                Department = _user.Branches.First().Department,
                Year = 2020,
                Week = 50,
                StartTime = TimeSpan.FromHours(15),
                EndTime = TimeSpan.FromHours(17),
                Date = ISOWeek.ToDateTime(2020,50, DayOfWeek.Monday)
            };

            var result = await _controller.SaveShift(_branch.Id, model);
            var redirectResult = result as RedirectToActionResult;
            
            Assert.IsNotNull(redirectResult);
            Assert.IsNotNull(_controller.TempData["alertMessage"]);
            
            var expectedRedirectValues = new RouteValueDictionary
            {
                { "branchId", _branch.Id },
                { "year", model.Year },
                { "week", model.Week },
                { "department", model.Department }
            };
            
            Assert.AreEqual(nameof(ScheduleController.Week), redirectResult.ActionName);
            Assert.AreEqual(expectedRedirectValues, redirectResult.RouteValues);

            var updatedShift = await Wrapper.Shift.Get(shift1 => shift1.Id == shift.Id);
            
            Assert.NotNull(updatedShift);
            
            Assert.AreEqual(TimeSpan.FromHours(15), updatedShift.StartTime);
            Assert.AreEqual(TimeSpan.FromHours(17), updatedShift.EndTime);
        }
    }
}