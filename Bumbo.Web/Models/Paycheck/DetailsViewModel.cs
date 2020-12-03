using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Bumbo.Logic.EmployeeRules;

namespace Bumbo.Web.Models.Paycheck
{
    public class DetailsViewModel
    {
        public Dictionary<int, List<Shift>> WeekShifts { get; set; }

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

        public class Shift
        {
            [Display(Name = "StartTime")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan StartTime { get; set; }

            [Display(Name="EndTime")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan EndTime { get; set; }

            [Display(Name = "DayOfWeek")]
            public DayOfWeek Day { get; set; }

            //TODO: DIsplayname
            [DisplayName("BreakTime")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan BreakTime => BreakDuration.GetDuration(TotalTime);

            [DisplayName("WorkTime")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan WorkTime => TotalTime.Subtract(BreakTime);

            [DisplayName("TotalTime")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan TotalTime => EndTime.Subtract(StartTime);

            [DisplayName("Difference")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan Difference { get; set; }
        }
    }
}
