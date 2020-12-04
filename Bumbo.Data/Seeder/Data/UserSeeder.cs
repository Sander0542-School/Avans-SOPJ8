using System;
using System.Collections.Generic;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Data.Seeder.Data
{
    public class UserSeeder : ISeeder<User>
    {
        public List<User> Get()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    
                    FirstName = "Bob",
                    MiddleName = "de",
                    LastName = "Vuller",

                    UserName = "bob.devuller@bumbo.test",

                    Email = "bob.devuller@bumbo.test",
                    EmailConfirmed = true,
                    
                    PhoneNumber = "0670668768",
                    PhoneNumberConfirmed = true,

                    Birthday = DateTime.Today.AddYears(-30),

                    ZipCode = "7452 NG",
                    HouseNumber = "4",


                    Contracts = new List<UserContract>
                    {
                        new UserContract
                        {
                            Function = "Vuller",
                            Scale = 8,
                            StartDate = DateTime.Today.AddMonths(-10),
                            EndDate = DateTime.Today.AddMonths(-5).AddDays(-1)
                        },
                        new UserContract
                        {
                            Function = "Vuller",
                            Scale = 6,
                            StartDate = DateTime.Today.AddMonths(-5),
                            EndDate = DateTime.Today.AddMonths(3)
                        }
                    },
                    
                    Branches = new List<UserBranch>
                    {
                        new UserBranch
                        {
                            Department = Department.VAK,
                            BranchId = 1
                        },
                        new UserBranch
                        {
                            Department = Department.VAK,
                            BranchId = 2
                        }
                    },
                    
                    UserAvailabilities = new List<UserAvailability>
                    {
                        new UserAvailability
                        {
                            Day = DayOfWeek.Monday,
                            StartTime = TimeSpan.FromHours(16),
                            EndTime = TimeSpan.FromHours(21),
                        },
                        new UserAvailability
                        {
                            Day = DayOfWeek.Tuesday,
                            StartTime = TimeSpan.FromHours(18),
                            EndTime = TimeSpan.FromHours(21),
                        },
                        new UserAvailability
                        {
                            Day = DayOfWeek.Thursday,
                            StartTime = TimeSpan.FromHours(16),
                            EndTime = TimeSpan.FromHours(21),
                        },
                        new UserAvailability
                        {
                            Day = DayOfWeek.Saturday,
                            StartTime = TimeSpan.FromHours(8),
                            EndTime = TimeSpan.FromHours(16),
                        },
                        new UserAvailability
                        {
                            Day = DayOfWeek.Sunday,
                            StartTime = TimeSpan.FromHours(12),
                            EndTime = TimeSpan.FromHours(20),
                        },
                    },
                    
                    ClockSystemTags = new List<ClockSystemTag>
                    {
                        new ClockSystemTag
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


                    Contracts = new List<UserContract>
                    {
                        new UserContract
                        {
                            Function = "Manager",
                            Scale = 20,
                            StartDate = DateTime.Today.AddMonths(-13),
                            EndDate = DateTime.Today.AddMonths(20).AddDays(-1)
                        },
                    },
                    
                    Branches = new List<UserBranch>
                    {
                        new UserBranch
                        {
                            Department = Department.KAS,
                            BranchId = 1
                        },
                    },
                    
                    ManagerBranches = new List<BranchManager>(
                        new List<BranchManager>{
                            new BranchManager
                            {
                                BranchId = 1,
                                UserId = 2,
                            }
                        })
                }
            };
        }
    }
}
