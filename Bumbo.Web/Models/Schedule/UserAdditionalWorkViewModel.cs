using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models;

namespace Bumbo.Web.Models.Schedule
{
    public class UserAdditionalWorkViewModel : ValidationAttribute
    {
        public List<UserAdditionalWork> Schedule { get; set; }
        public UserAdditionalWork Work { get; set; }

        public ValidationResult Validate(ValidationContext validationContext)
        {
            var additionalWork = (UserAdditionalWork)validationContext.ObjectInstance;

            if (additionalWork.StartTime > additionalWork.EndTime)
            {
                return new ValidationResult("Start time must be before end time");
            }

            return ValidationResult.Success;
        }

        public class InputAdditionalWork : AdditionalWork
        {
            public static readonly DayOfWeek[] DaysOfWeek =
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday,
                DayOfWeek.Sunday
            };
        }

        public abstract class AdditionalWork
        {
            public int UserId { get; set; }

            [Required]
            [Display(Name = "Day")]
            public DayOfWeek Day { get; set; }

            [Required]
            [Display(Name = "Start Time")]
            [DataType(DataType.Time)]
            public TimeSpan StartTime { get; set; }

            [Required]
            [Display(Name = "End Time")]
            [DataType(DataType.Time)]
            public TimeSpan EndTime { get; set; }
        }
    }
}
