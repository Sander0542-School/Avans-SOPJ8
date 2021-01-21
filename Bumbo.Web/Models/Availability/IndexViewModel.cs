using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.Extensions.Localization;
namespace Bumbo.Web.Models.Availability
{
    public class IndexViewModel
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

        [Required]
        public List<Availability> Schedule { get; set; }

        public class Availability : IValidatableObject
        {
            [Required]
            public DayOfWeek Day { get; set; }

            [Display(Name = "Start Time")]
            [DataType(DataType.Time)]
            public TimeSpan? StartTime { get; set; }

            [Display(Name = "End Time")]
            [DataType(DataType.Time)]
            public TimeSpan? EndTime { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                var localizer = (IStringLocalizer)validationContext.GetService(typeof(IStringLocalizer<Availability>));

                if (StartTime.HasValue && EndTime.HasValue)
                {
                    if (StartTime > EndTime)
                    {
                        yield return new ValidationResult(localizer["The start time cannot be after the end time on {0}.", ISOWeek.ToDateTime(1, 1, Day).ToString("dddd")]);
                    }
                }

                if ((!StartTime.HasValue && EndTime.HasValue) || (!EndTime.HasValue && StartTime.HasValue))
                {
                    yield return new ValidationResult(localizer["The start and end time both need to be filled in on {0}.", ISOWeek.ToDateTime(1, 1, Day).ToString("dddd")]);
                }
            }
        }
    }
}
