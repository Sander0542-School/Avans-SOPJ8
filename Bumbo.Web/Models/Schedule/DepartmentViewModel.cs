using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Bumbo.Logic.EmployeeRules;
using Microsoft.AspNetCore.Mvc;

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

        public InputShift Input { get; set; }

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

        public class InputShift
        {
            public int? ShiftId { get; set; }

            public int UserId { get; set; }

            [DisplayName("Medewerker")]
            public string UserName { get; set; }

            [DisplayName("Afdeling")]
            public Department Department { get; set; }

            [DisplayName("Datum")]
            [DataType(DataType.Date)]
            public DateTime Date { get; set; }

            [DisplayName("Starttijd")]
            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan StartTime { get; set; }

            [DisplayName("Eindtijd")]
            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan EndTime { get; set; }
        }
    }
}