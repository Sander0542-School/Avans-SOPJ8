using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models;
using Bumbo.Logic.EmployeeRules;
namespace Bumbo.Web.Models.Paycheck
{
    public class PaycheckViewModel
    {
        public Dictionary<User, List<Shift>> MonthShifts { get; set; }
        public List<int> WeekNumbers { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public Branch Branch { get; set; }
        public bool OverviewApproved { get; set; }

        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Scale { get; set; }
            public string Function { get; set; }
        }

        public class Shift
        {
            [Display(Name = "Start Time")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan StartTime { get; set; }

            [Display(Name = "End Time")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan EndTime { get; set; }

            [Display(Name = "Day of Week")]
            public DayOfWeek Day { get; set; }

            [Display(Name = "Break Time")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan BreakTime => BreakDuration.GetDuration(TotalTime);

            [Display(Name = "Work Time")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan WorkTime => TotalTime.Subtract(BreakTime);

            [Display(Name = "Total Time")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan TotalTime => EndTime.Subtract(StartTime);

            public int Week { get; set; }
        }
    }
}
