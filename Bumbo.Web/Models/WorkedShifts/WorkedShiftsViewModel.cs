using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Bumbo.Data.Models;
namespace Bumbo.Web.Models.WorkedShifts
{
    public class WorkedShiftsViewModel
    {

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

        private DateTime _mondayOfWeek => ISOWeek.ToDateTime(Year, Week, DayOfWeek.Monday);

        public int NextWeek => ISOWeek.GetWeekOfYear(_mondayOfWeek.AddDays(7));
        public int NextYear => _mondayOfWeek.AddDays(7).Year;
        public int PreviousWeek => ISOWeek.GetWeekOfYear(_mondayOfWeek.AddDays(-7));
        public int PreviousYear => _mondayOfWeek.AddDays(-7).Year;

        [Display(Name = "Year")]
        public int Year { get; set; }

        [Display(Name = "Week")]
        public int Week { get; set; }

        public List<WorkedShift> WorkedShifts { get; set; }

        public Branch Branch { get; set; }
    }
}
