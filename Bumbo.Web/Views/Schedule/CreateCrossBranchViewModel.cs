using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Bumbo.Logic.EmployeeRules;

namespace Bumbo.Web.Views.Schedule
{
    public class CreateCrossBranchViewModel
    {
        public class Shift
        {
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

            public User User;
        }

    }
}
