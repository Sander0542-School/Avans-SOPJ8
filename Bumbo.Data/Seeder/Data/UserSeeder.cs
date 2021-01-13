using System;
using System.Collections.Generic;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Bumbo.Data.Seeder.Data
{
    public class UserSeeder : ISeeder<User>
    {
        public List<User> Get()
        {
            return new List<User>
            {
                new()
                {
                    Id = TestDataSeeder.SuperId,
                    FirstName = "Super",
                    LastName = "Test",
                    UserName = "super@bumbo.test",
                    Email = "super@bumbo.test",
                    EmailConfirmed = true,
                    Birthday = DateTime.Today.AddYears(-25),
                    ZipCode = "7741 dn",
                    HouseNumber = "18a"
                },
                new()
                {
                    Id = TestDataSeeder.ManagerId,
                    FirstName = "Manager",
                    LastName = "Test",
                    UserName = "manager@bumbo.test",
                    Email = "manager@bumbo.test",
                    EmailConfirmed = true,
                    PhoneNumber = "0670665768",
                    PhoneNumberConfirmed = true,
                    Birthday = DateTime.Today.AddYears(-40),
                    ZipCode = "5454 NG",
                    HouseNumber = "4"
                },
                new()
                {
                    Id = TestDataSeeder.EmployeeId,
                    FirstName = "Employee",
                    LastName = "Test",
                    UserName = "employee@bumbo.test",
                    Email = "employee@bumbo.test",
                    EmailConfirmed = true,
                    PhoneNumber = "0670668768",
                    PhoneNumberConfirmed = true,
                    Birthday = DateTime.Today.AddYears(-30),
                    ZipCode = "7452 ng",
                    HouseNumber = "4"
                }
            };
        }

        public List<UserAvailability> GetAvailabilities()
        {
            return new List<UserAvailability>
            {
                new()
                {
                    UserId = TestDataSeeder.EmployeeId,
                    Day = DayOfWeek.Monday,
                    StartTime = TimeSpan.FromHours(16),
                    EndTime = TimeSpan.FromHours(21),
                },
                new()
                {
                    UserId = TestDataSeeder.EmployeeId,
                    Day = DayOfWeek.Tuesday,
                    StartTime = TimeSpan.FromHours(18),
                    EndTime = TimeSpan.FromHours(21),
                },
                new()
                {
                    UserId = TestDataSeeder.EmployeeId,
                    Day = DayOfWeek.Thursday,
                    StartTime = TimeSpan.FromHours(16),
                    EndTime = TimeSpan.FromHours(21),
                },
                new()
                {
                    UserId = TestDataSeeder.EmployeeId,
                    Day = DayOfWeek.Saturday,
                    StartTime = TimeSpan.FromHours(8),
                    EndTime = TimeSpan.FromHours(16),
                },
                new()
                {
                    UserId = TestDataSeeder.EmployeeId,
                    Day = DayOfWeek.Sunday,
                    StartTime = TimeSpan.FromHours(12),
                    EndTime = TimeSpan.FromHours(20),
                }
            };
        }

        public List<ClockSystemTag> GetClockSystemTags()
        {
            return new List<ClockSystemTag>
            {
                new()
                {
                    UserId = TestDataSeeder.EmployeeId, SerialNumber = "74:a3:c5:c4:88:2b"
                }
            };
        }
        public List<UserContract> GetContracts()
        {
            return new List<UserContract>
            {
                new()
                {
                    UserId = TestDataSeeder.SuperId,
                    Function = "Owner",
                    Scale = 9,
                    StartDate = DateTime.Today.AddMonths(-27),
                    EndDate = DateTime.Today.AddMonths(5).AddDays(-14)
                },
                new()
                {
                    UserId = TestDataSeeder.ManagerId,
                    Function = "Manager",
                    Scale = 5,
                    StartDate = DateTime.Today.AddMonths(-13),
                    EndDate = DateTime.Today.AddMonths(20).AddDays(-1)
                },
                new()
                {
                    UserId = TestDataSeeder.EmployeeId,
                    Function = "Vuller",
                    Scale = 8,
                    StartDate = DateTime.Today.AddMonths(-10),
                    EndDate = DateTime.Today.AddMonths(-5).AddDays(-1)
                },
                new()
                {
                    UserId = TestDataSeeder.EmployeeId,
                    Function = "Vuller",
                    Scale = 6,
                    StartDate = DateTime.Today.AddMonths(-5),
                    EndDate = DateTime.Today.AddMonths(3)
                }
            };
        }
    }
}
