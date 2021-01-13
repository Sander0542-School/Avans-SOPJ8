using System;
using System.Collections.Generic;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace Bumbo.Data.Seeder.Data
{
    public class UserSeeder : ISeeder<User>
    {
        public List<User> Get()
        {
            return new()
            {
                new User
                {
                    Id = 3,
                    FirstName = "Bob",
                    MiddleName = "de",
                    LastName = "Vuller",
                    UserName = "bob.devuller@bumbo.test",
                    Email = "bob.devuller@bumbo.test",
                    EmailConfirmed = true,
                    PhoneNumber = "0670668768",
                    PhoneNumberConfirmed = true,
                    Birthday = DateTime.Today.AddYears(-30),
                    ZipCode = "7452 ng",
                    HouseNumber = "4",
                    PasswordHash = "AQAAAAEAACcQAAAAEF1LEjqfSyH32xHhjoDkymshX56PAHghxad0/sMyw1QJVD2UdJv8paV6QlYHaYkS3g==", // Pass1234!
                    Contracts = new List<UserContract>
                    {
                        new()
                        {
                            Function = "Vuller",
                            Scale = 8,
                            StartDate = DateTime.Today.AddMonths(-10),
                            EndDate = DateTime.Today.AddMonths(-5).AddDays(-1)
                        },
                        new()
                        {
                            Function = "Vuller",
                            Scale = 6,
                            StartDate = DateTime.Today.AddMonths(-5),
                            EndDate = DateTime.Today.AddMonths(3)
                        }
                    },
                    Branches = new List<UserBranch>
                    {
                        new()
                        {
                            Department = Department.VAK, BranchId = 1
                        },
                        new()
                        {
                            Department = Department.VAK, BranchId = 2
                        }
                    },
                    UserAvailabilities = new List<UserAvailability>
                    {
                        new()
                        {
                            Day = DayOfWeek.Monday,
                            StartTime = TimeSpan.FromHours(16),
                            EndTime = TimeSpan.FromHours(21),
                        },
                        new()
                        {
                            Day = DayOfWeek.Tuesday,
                            StartTime = TimeSpan.FromHours(18),
                            EndTime = TimeSpan.FromHours(21),
                        },
                        new()
                        {
                            Day = DayOfWeek.Thursday,
                            StartTime = TimeSpan.FromHours(16),
                            EndTime = TimeSpan.FromHours(21),
                        },
                        new()
                        {
                            Day = DayOfWeek.Saturday,
                            StartTime = TimeSpan.FromHours(8),
                            EndTime = TimeSpan.FromHours(16),
                        },
                        new()
                        {
                            Day = DayOfWeek.Sunday,
                            StartTime = TimeSpan.FromHours(12),
                            EndTime = TimeSpan.FromHours(20),
                        },
                    },
                    ClockSystemTags = new List<ClockSystemTag>
                    {
                        new()
                        {
                            SerialNumber = "74:a3:c5:c4:88:2b"
                        }
                    }
                },
                new User
                {
                    Id = 2,
                    FirstName = "Micheal",
                    MiddleName = "van",
                    LastName = "Managum",
                    UserName = "micheal.sexybabes@bumbo.test",
                    Email = "micheal.sexybabes@bumbo.test",
                    EmailConfirmed = true,
                    PhoneNumber = "0670665768",
                    PhoneNumberConfirmed = true,
                    Birthday = DateTime.Today.AddYears(-40),
                    ZipCode = "5454 NG",
                    HouseNumber = "4",
                    PasswordHash = "AQAAAAEAACcQAAAAEF1LEjqfSyH32xHhjoDkymshX56PAHghxad0/sMyw1QJVD2UdJv8paV6QlYHaYkS3g==", // Pass1234!
                    Contracts = new List<UserContract>
                    {
                        new()
                        {
                            Function = "Manager",
                            Scale = 20,
                            StartDate = DateTime.Today.AddMonths(-13),
                            EndDate = DateTime.Today.AddMonths(20).AddDays(-1)
                        },
                    },
                    Branches = new List<UserBranch>
                    {
                        new()
                        {
                            Department = Department.KAS, BranchId = 1
                        },
                    },
                    ManagerBranches = new List<BranchManager>(
                    new List<BranchManager>
                    {
                        new()
                        {
                            BranchId = 1, UserId = 2,
                        }
                    })
                },
                new User
                {
                    Id = 1,
                    FirstName = "Bumbo",
                    LastName = "Super",
                    UserName = "super@bumbo.test",
                    Email = "super@bumbo.test",
                    EmailConfirmed = true,
                    PhoneNumber = "0637264524",
                    PhoneNumberConfirmed = true,
                    Birthday = DateTime.Today.AddYears(-25),
                    ZipCode = "7741 dn",
                    HouseNumber = "18a",
                    PasswordHash = "AQAAAAEAACcQAAAAEF1LEjqfSyH32xHhjoDkymshX56PAHghxad0/sMyw1QJVD2UdJv8paV6QlYHaYkS3g==", // Pass1234!
                    Contracts = new List<UserContract>
                    {
                        new()
                        {
                            Function = "Manager",
                            Scale = 20,
                            StartDate = DateTime.Today.AddMonths(-13),
                            EndDate = DateTime.Today.AddMonths(20).AddDays(-1)
                        },
                    },
                    Branches = new List<UserBranch>
                    {
                        new()
                        {
                            Department = Department.KAS, BranchId = 1
                        },
                    },
                    ManagerBranches = new List<BranchManager>(
                    new List<BranchManager>
                    {
                        new()
                        {
                            BranchId = 1, UserId = 2,
                        }
                    })
                }
            };
        }
        public List<IdentityUserRole<int>> GetRoles()
        {
            return new()
            {
                new()
                {
                    RoleId = 1, UserId = 1
                }
            };
        }
    }
}
