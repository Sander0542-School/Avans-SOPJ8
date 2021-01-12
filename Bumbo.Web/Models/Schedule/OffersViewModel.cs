using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models.Enums;
namespace Bumbo.Web.Models.Schedule
{
    public class OffersViewModel
    {
        public Dictionary<DateTime, List<Shift>> Shifts { get; set; }

        public InputModel Input { get; set; }

        public class Shift
        {
            public int Id { get; set; }

            public bool OwnedShift { get; set; }

            [Display(Name = "Employee")]
            public string Employee { get; set; }

            [Display(Name = "Department")]
            public Department Department { get; set; }

            [Display(Name = "Start Time")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan StartTime { get; set; }

            [Display(Name = "End Time")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan EndTime { get; set; }
        }

        public class InputModel
        {
            public int ShiftId { get; set; }
        }
    }
}
