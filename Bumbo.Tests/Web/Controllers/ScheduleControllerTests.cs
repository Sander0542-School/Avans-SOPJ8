using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Bumbo.Web.Controllers;
using Bumbo.Web.Models.Schedule;
using NUnit.Framework;

namespace Bumbo.Tests.Web.Controllers
{
    public class ScheduleControllerTests : ControllerTestBase<ScheduleController>
    {
        private User _user;
        private Branch _branch;

        private ScheduleController _controller;

        [SetUp]
        public async void Setup()
        {
            _controller = new ScheduleController(Wrapper, Localizer);
            
            _branch = await Wrapper.Branch.Add(new Branch
            {
                Id = 1,
                Name = "Test Bumbo",
                ZipCode = "1234 AB",
                HouseNumber = "10a"
            });

            _user = await Wrapper.User.Add(new User
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
            });
        }

        [Test]
        public async void TestAddShift()
        {
            var model = new DepartmentViewModel.InputShiftModel
            {
                Department = _user.Branches.First().Department,
                Year = 2020,
                Week = 50,
                StartTime = new TimeSpan(16,0,0),
                EndTime = new TimeSpan(18,0,0),
                Date = ISOWeek.ToDateTime(2020,50, DayOfWeek.Monday),
            };

            var result = await _controller.SaveShift(_branch.Id, model);
            
            
        }
    }
}