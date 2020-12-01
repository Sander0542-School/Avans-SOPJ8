using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Microsoft.VisualBasic.CompilerServices;

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

        public EditForecastViewModel EditForecast;


        public class EditForecastViewModel
        {
            public int BranchId;
            public int Week;
            public int Year;

            [DataType(DataType.Date)]
            [Required]
            [DisplayName("Date")]
            public DateTime Date { get; set; }

            [Display(Name = "Department")]
            [Required]
            public Department Department { get; set; }

            [Display(Name = "Working hours")]
            [Required]
            [Range(1, 500)]
            public int Hours { get; set; }

            [Display(Name = "Minutes")]
            [Required]
            [Range(1, 60)]
            public int Minutes { get; set; }
        }
    }
}