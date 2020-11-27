using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Bumbo.Logic.EmployeeRules;

namespace Bumbo.Web.Models.Schedule
{
    public class DepartmentViewModel
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
        public Department Department { get; set; }

        public InputShiftModel InputShift { get; set; }

        public InputCopyWeekModel InputCopyWeek { get; set; }

        public InputApproveScheduleModel InputApproveSchedule { get; set; }

        public List<EmployeeShift> EmployeeShifts { get; set; }

        public bool ScheduleApproved { get; set; }

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
            public int UserId { get; set; }

            [DisplayName("Name")]
            public string Name { get; set; }

            [DisplayName("Contract")]
            public string Contract { get; set; }

            [DisplayName("Scale")]
            public int Scale { get; set; }

            [DisplayName("MaxHours")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan MaxHours { get; set; }

            [DisplayName("PlannedTime")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan PlannedTime => new TimeSpan(Shifts.Sum(shift => shift.WorkTime.Ticks));

            public List<Shift> Shifts { get; set; }
        }

        public class Shift
        {
            public int Id { get; set; }

            [DisplayName("Date")]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
            public DateTime Date { get; set; }

            [DisplayName("StartTime")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan StartTime { get; set; }

            [DisplayName("EndTime")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan EndTime { get; set; }

            [DisplayName("Notifications")]
            public IEnumerable<string> Notifications { get; set; }

            [DisplayName("BreakTime")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan BreakTime => BreakDuration.GetDuration(TotalTime);

            [DisplayName("WorkTime")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan WorkTime => TotalTime.Subtract(BreakTime);

            [DisplayName("TotalTime")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan TotalTime => EndTime.Subtract(StartTime);
        }

        public class InputShiftModel : InputDateDepartmentModel
        {
            public int? ShiftId { get; set; }

            [Required]
            public int UserId { get; set; }

            [DisplayName("Employee")]
            [Required]
            public string UserName { get; set; }

            [DisplayName("Date")]
            [DataType(DataType.Date)]
            [Required]
            public DateTime Date { get; set; }

            [DisplayName("StartTime")]
            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            [Required]
            public TimeSpan StartTime { get; set; }

            [DisplayName("EndTime")]
            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            [Required]
            public TimeSpan EndTime { get; set; }
        }

        public class InputCopyWeekModel : InputDateDepartmentModel
        {
            [DisplayName("Year")]
            public int TargetYear { get; set; }

            [DisplayName("Week")]
            public int TargetWeek { get; set; }

            [DisplayName("Date")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
            public DateTime TargetDate => ISOWeek.ToDateTime(TargetYear, TargetWeek, DayOfWeek.Monday);
        }

        public class InputApproveScheduleModel : InputDateDepartmentModel
        {
        }

        public abstract class InputDateDepartmentModel
        {
            [Display(Name = "Year")]
            [Required]
            public int Year { get; set; }

            [Display(Name = "Week")]
            [Required]
            public int Week { get; set; }

            [Display(Name = "Department")]
            [Required]
            public Department Department { get; set; }
        }
    }
}