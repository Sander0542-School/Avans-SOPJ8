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
        [DisplayName("Jaar")]
        public int Year { get; set; }

        [DisplayName("Week")]
        [DisplayFormat(DataFormatString = "Week {0}")]
        public int Week { get; set; }

        [DisplayName("Afdeling")]
        public Department Department { get; set; }

        public InputShiftModel InputShift { get; set; }

        public InputCopyWeekModel InputCopyWeek { get; set; }

        public InputApproveScheduleModel InputApproveSchedule { get; set; }

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
            public int UserId { get; set; }

            [DisplayName("Naam")]
            public string Name { get; set; }

            [DisplayName("Contract")]
            public string Contract { get; set; }

            [DisplayName("Schaal")]
            public int Scale { get; set; }

            [DisplayName("Maximale uren")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan MaxHours { get; set; }

            [DisplayName("Ingeplande tijd")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan PlannedTime => new TimeSpan(Shifts.Sum(shift => shift.WorkingTime.Ticks));

            public List<Shift> Shifts { get; set; }
        }

        public class Shift
        {
            public int Id { get; set; }

            [DisplayName("Datum")]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
            public DateTime Date { get; set; }

            [DisplayName("Starttijd")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan StartTime { get; set; }

            [DisplayName("Eindtijd")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan EndTime { get; set; }

            [DisplayName("Meldingen")]
            public IEnumerable<string> Notifications { get; set; }

            [DisplayName("Pauzeduur")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan BreakTime => BreakDuration.GetDuration(TotalTime);

            [DisplayName("Werkduur")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan WorkingTime => TotalTime.Subtract(BreakTime);

            [DisplayName("Totale duur")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan TotalTime => EndTime.Subtract(StartTime);
        }

        public class InputShiftModel : InputDateDepartmentModel
        {
            public int? ShiftId { get; set; }

            [Required]
            public int UserId { get; set; }

            [DisplayName("Medewerker")]
            [Required]
            public string UserName { get; set; }

            [DisplayName("Datum")]
            [DataType(DataType.Date)]
            [Required]
            public DateTime Date { get; set; }

            [DisplayName("Starttijd")]
            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            [Required]
            public TimeSpan StartTime { get; set; }

            [DisplayName("Eindtijd")]
            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            [Required]
            public TimeSpan EndTime { get; set; }
        }

        public class InputCopyWeekModel : InputDateDepartmentModel
        {
            [DisplayName("Jaar")]
            public int TargetYear { get; set; }

            [DisplayName("Week")]
            public int TargetWeek { get; set; }

            [DisplayName("Datum")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
            public DateTime TargetDate => ISOWeek.ToDateTime(TargetYear, TargetWeek, DayOfWeek.Monday);
        }

        public class InputApproveScheduleModel : InputDateDepartmentModel
        {
        }

        public abstract class InputDateDepartmentModel
        {
            [DisplayName("Jaar")]
            [Required]
            public int Year { get; set; }

            [DisplayName("Week")]
            [Required]
            public int Week { get; set; }

            [DisplayName("Afdeling")]
            [Required]
            public Department Department { get; set; }
        }
    }
}