using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
namespace Bumbo.Web.Models.Schedule
{
    public class CrossBranchViewModel
    {
        public class Shift
        {

            public Branch Branch;

            public User User;
            public int Id { get; set; }

            [DisplayName("Date")]
            [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
            public DateTime Date { get; set; }

            [DisplayName("Start Time")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan StartTime { get; set; }

            [DisplayName("End Time")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan EndTime { get; set; }

            [DisplayName("Total Time")]
            [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
            public TimeSpan TotalTime => EndTime.Subtract(StartTime);

            [DisplayName("Department")]
            public Department Department { get; set; }
        }

        public class AdoptShift
        {
            public List<User> AvailableEmployees { get; set; }
            public Shift Shift { get; set; }

            public int ShiftId { get; set; }
            public int SelectedUserId { get; set; }
        }
    }
}
