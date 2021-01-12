using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bumbo.Web.Models.Forecast;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;


namespace Bumbo.Web.Models.Furlough
{
    public class FurloughViewModel
    {
        public List<Data.Models.Furlough> Furloughs { get; set; }

        public InputModel Input { get; set; }

        public class InputModel : IValidatableObject
        {
            public int? Id { get; set; }

            [Display(Name = "Description")]
            [Required]
            public string Description { get; set; }

            [Display(Name = "StartDate")]
            [DataType(DataType.Date)]
            [Required]
            public DateTime StartDate { get; set; }

            [Display(Name = "EndDate")]
            [DataType(DataType.Date)]
            [Required]
            public DateTime EndDate { get; set; }

            [Display(Name = "StartTime")]
            [DataType(DataType.Time)]
            public TimeSpan? StartTime { get; set; }

            [Display(Name = "EndTime")]
            [DataType(DataType.Time)]
            public TimeSpan? EndTime { get; set; }

            [Display(Name = "IsAllDay")]
            public bool IsAllDay { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                var localizer = validationContext.GetService<IStringLocalizer<ForecastViewModel>>();

                if (StartDate < DateTime.Today)
                {
                    yield return new ValidationResult(localizer["The start date has to be in the future"], new[] { "StartDate" });
                }

                if (EndDate < StartDate)
                {
                    yield return new ValidationResult(localizer["The end date cannot be greater than we start date"], new[] { "StartDate", "EndDate" });
                }

                if (!IsAllDay && EndTime < StartTime)
                {
                    yield return new ValidationResult(localizer["The end time cannot be greater than we start time"], new[] { "StartTime", "EndTime" });
                }
            }
        }
    }
}
