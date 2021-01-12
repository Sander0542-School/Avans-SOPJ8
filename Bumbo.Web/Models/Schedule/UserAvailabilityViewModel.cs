﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models;
namespace Bumbo.Web.Models.Schedule
{
    public class UserAvailabilityViewModel : ValidationAttribute
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

        public List<UserAvailability> Schedule { get; set; }
        public UserAvailability Availability { get; set; }

        public ValidationResult Validate(ValidationContext validationContext)
        {
            var availability = (UserAvailability)validationContext.ObjectInstance;

            if (availability.StartTime > availability.EndTime)
            {
                return new ValidationResult("Starttime must be before endtime");
            }

            return ValidationResult.Success;
        }
    }
}
