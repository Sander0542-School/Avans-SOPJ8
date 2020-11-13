using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Bumbo.Data.Models;

namespace Bumbo.Web.Models.Schedule
{
    public class DepartmentViewModel
    {
        public int Year { get; set; }
        public int Week { get; set; }

        public List<EmployeeShift> EmployeeShifts { get; set; }

        public Branch Branch { get; set; }

        public readonly DayOfWeek[] DaysOfWeek =
        {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday,
            DayOfWeek.Saturday,
            DayOfWeek.Sunday
        };

        public class EmployeeShift
        {
            public string Name { get; set; }
            
            public string Contract { get; set; }
            
            [DisplayFormat(DataFormatString = "{0:C}")]
            public double Kpu { get; set; }
            
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan MaxHours { get; set; }
            
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan PlannedTime => new TimeSpan(0, (int) Shifts.Sum(shift => shift.WorkingTime.TotalMinutes), 0);

            public List<Shift> Shifts { get; set; }
        }

        public class Shift
        {
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public DateTime StartTime { get; set; }
            
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public DateTime EndTime { get; set; }

            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan BreakTime
            {
                get
                {
                    if (TotalTime.TotalMinutes > 480)
                    {
                        return new TimeSpan(0, 60, 0);
                    }

                    if (TotalTime.TotalMinutes > 270)
                    {
                        return new TimeSpan(0, 30, 0);
                    }

                    return new TimeSpan(0, 0, 0);
                }
            }

            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan WorkingTime => TotalTime.Subtract(BreakTime);
            
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan TotalTime => EndTime.Subtract(StartTime);
        }
    }
}