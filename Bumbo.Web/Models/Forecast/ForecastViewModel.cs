using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Web.Models.Forecast
{
    public class ForecastViewModel
    {
        private DateTime _mondayOfWeek => ISOWeek.ToDateTime(Year, Week, DayOfWeek.Monday);

        public int NextWeek => ISOWeek.GetWeekOfYear(_mondayOfWeek.AddDays(7));
        public int NextYear => _mondayOfWeek.AddDays(7).Year;
        public int PreviousWeek => ISOWeek.GetWeekOfYear(_mondayOfWeek.AddDays(-7));
        public int PreviousYear => _mondayOfWeek.AddDays(-7).Year;

        [Display(Name = "Year")]
        public int Year { get; set; }

        [Display(Name = "Week")]
        public int Week { get; set; }

        [Display(Name = "Department")]
        public Department? Department { get; set; }

        [Display(Name = "Branch")]
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

        public IEnumerable<Data.Models.Forecast> Forecasts;

    }
}