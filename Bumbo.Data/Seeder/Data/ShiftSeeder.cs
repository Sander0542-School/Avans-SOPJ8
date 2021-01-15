using System;
using System.Collections.Generic;
using System.Globalization;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Data.Seeder.Data
{
    public class ShiftSeeder : ISeeder<BranchSchedule>
    {
        public List<BranchSchedule> Get()
        {
            var year = DateTime.Today.Year;
            var week = ISOWeek.GetWeekOfYear(DateTime.Today);
            
            return new List<BranchSchedule>
            {
                new BranchSchedule
                {
                    BranchId = 1,
                    Department = Department.VAK,
                    Year = year,
                    Week = week,
                    Confirmed = false,
                    
                    
                    Shifts = new List<Shift>
                    {
                        new Shift
                        {
                            UserId = 1,
                            
                            Date = ISOWeek.ToDateTime(year, week, DayOfWeek.Monday),
                            StartTime = TimeSpan.FromHours(17),
                            EndTime = TimeSpan.FromHours(21),
                            
                            WorkedShift = new WorkedShift
                            {
                                StartTime = TimeSpan.FromHours(17),
                                EndTime = TimeSpan.FromHours(21).Add(TimeSpan.FromMinutes(30)),
                                Sick = false
                            }
                        },
                        new Shift
                        {
                            UserId = 1,
                            
                            Date = ISOWeek.ToDateTime(year, week, DayOfWeek.Tuesday),
                            StartTime = TimeSpan.FromHours(18),
                            EndTime = TimeSpan.FromHours(21),
                            
                            WorkedShift = new WorkedShift
                            {
                                StartTime = TimeSpan.FromHours(17),
                                EndTime = TimeSpan.FromHours(21),
                                Sick = true
                            }
                        },
                        new Shift
                        {
                            UserId = 1,
                            
                            Date = ISOWeek.ToDateTime(year, week, DayOfWeek.Saturday),
                            StartTime = TimeSpan.FromHours(10),
                            EndTime = TimeSpan.FromHours(14)
                        },
                        new Shift
                        {
                            UserId = 1,
                            
                            Date = ISOWeek.ToDateTime(year, week, DayOfWeek.Sunday),
                            StartTime = TimeSpan.FromHours(14),
                            EndTime = TimeSpan.FromHours(18)
                        }
                    }
                }
            };
        }
    }
}