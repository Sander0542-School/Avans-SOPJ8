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
using Moq;
using NUnit.Framework;
namespace Bumbo.Tests.Web.Controllers
{
    public class ScheduleControllerTests : ControllerTestBase<ScheduleController>
    {
        private Branch _branch;

        private ScheduleController _controller;
        private User _user;

        [SetUp]
        public void Setup()
        {
            _controller = new ScheduleController(Wrapper, Localizer, UserManager)
            {
                TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            };
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
                    new()
                    {
                        Department = Department.VAK, BranchId = _branch.Id
                    }
                },
                UserAvailabilities = new List<UserAvailability>
                {
                    new()
                    {
                        Day = DayOfWeek.Monday,
                        StartTime = TimeSpan.FromHours(15),
                        EndTime = TimeSpan.FromHours(21)
                    }
                }
            }).Result;
        }

        [Test] [Order(1)]
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
                Date = ISOWeek.ToDateTime(2020, 50, DayOfWeek.Monday)
            };

            var result = await _controller.SaveShift(_branch.Id, model);
            var redirectResult = result as RedirectToActionResult;

            Assert.IsNotNull(redirectResult);
            Assert.IsNotNull(_controller.TempData["AlertMessage"]);
            Assert.IsTrue(_controller.TempData["AlertMessage"].ToString()?.StartsWith("success") ?? false);

            var expectedRedirectValues = new RouteValueDictionary
            {
                {
                    "branchId", _branch.Id
                },
                {
                    "year", model.Year
                },
                {
                    "week", model.Week
                },
                {
                    "department", model.Department
                }
            };

            Assert.AreEqual(nameof(ScheduleController.Week), redirectResult.ActionName);
            Assert.AreEqual(expectedRedirectValues, redirectResult.RouteValues);

            var shift = await Wrapper.Shift.Get(shift1 => shift1.UserId == _user.Id);

            Assert.IsNotNull(shift);
            Assert.AreEqual(_user.Id, shift.UserId);
        }

        [Test] [Order(2)]
        public async Task TestEditShift()
        {
            var shift = await Wrapper.Shift.Get(shift1 => shift1.UserId == _user.Id);

            Assert.IsNotNull(shift);
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
                Date = ISOWeek.ToDateTime(2020, 50, DayOfWeek.Monday)
            };

            var result = await _controller.SaveShift(_branch.Id, model);
            var redirectResult = result as RedirectToActionResult;

            Assert.IsNotNull(redirectResult);
            Assert.IsNotNull(_controller.TempData["AlertMessage"]);
            Assert.IsTrue(_controller.TempData["AlertMessage"].ToString()?.StartsWith("success") ?? false);

            var expectedRedirectValues = new RouteValueDictionary
            {
                {
                    "branchId", _branch.Id
                },
                {
                    "year", model.Year
                },
                {
                    "week", model.Week
                },
                {
                    "department", model.Department
                }
            };

            Assert.AreEqual(nameof(ScheduleController.Week), redirectResult.ActionName);
            Assert.AreEqual(expectedRedirectValues, redirectResult.RouteValues);

            var updatedShift = await Wrapper.Shift.Get(shift1 => shift1.Id == shift.Id);

            Assert.IsNotNull(updatedShift);

            Assert.AreEqual(TimeSpan.FromHours(15), updatedShift.StartTime);
            Assert.AreEqual(TimeSpan.FromHours(17), updatedShift.EndTime);
        }

        [Test] [Order(3)]
        public async Task TestCopySchedule()
        {
            var model = new DepartmentViewModel.InputCopyWeekModel
            {
                Department = _user.Branches.First().Department,
                Year = 2020,
                Week = 50,
                TargetYear = 2020,
                TargetWeek = 51
            };

            var result = await _controller.CopySchedule(_branch.Id, model);
            var redirectResult = result as RedirectToActionResult;

            Assert.IsNotNull(redirectResult);
            Assert.IsNotNull(_controller.TempData["AlertMessage"]);
            Assert.IsTrue(_controller.TempData["AlertMessage"].ToString()?.StartsWith("success") ?? false);

            var expectedRedirectValues = new RouteValueDictionary
            {
                {
                    "branchId", _branch.Id
                },
                {
                    "year", model.TargetYear
                },
                {
                    "week", model.TargetWeek
                },
                {
                    "department", model.Department
                }
            };

            Assert.AreEqual(nameof(ScheduleController.Week), redirectResult.ActionName);
            Assert.AreEqual(expectedRedirectValues, redirectResult.RouteValues);

            var targetSchedule = await Wrapper.BranchSchedule.Get(
            schedule => schedule.Year == model.TargetYear,
            schedule => schedule.Week == model.TargetWeek,
            schedule => schedule.Department == model.Department);

            Assert.IsNotNull(targetSchedule);
            Assert.That(targetSchedule.Shifts, Has.Count.EqualTo(1));
        }

        [Test] [Order(3)]
        public async Task TestApproveSchedule()
        {
            var model = new DepartmentViewModel.InputApproveScheduleModel
            {
                Department = _user.Branches.First().Department,
                Year = 2020,
                Week = 50
            };

            var result = await _controller.ApproveSchedule(_branch.Id, model);
            var redirectResult = result as RedirectToActionResult;

            Assert.IsNotNull(redirectResult);
            Assert.IsNotNull(_controller.TempData["AlertMessage"]);
            Assert.IsTrue(_controller.TempData["AlertMessage"].ToString()?.StartsWith("success") ?? false);

            var expectedRedirectValues = new RouteValueDictionary
            {
                {
                    "branchId", _branch.Id
                },
                {
                    "year", model.Year
                },
                {
                    "week", model.Week
                },
                {
                    "department", model.Department
                }
            };

            Assert.AreEqual(nameof(ScheduleController.Week), redirectResult.ActionName);
            Assert.AreEqual(expectedRedirectValues, redirectResult.RouteValues);

            var schedule = await Wrapper.BranchSchedule.Get(
            schedule1 => schedule1.Year == model.Year,
            schedule1 => schedule1.Week == model.Week,
            schedule1 => schedule1.Department == model.Department);

            Assert.IsNotNull(schedule);
            Assert.IsTrue(schedule.Confirmed);
        }
    }
}
