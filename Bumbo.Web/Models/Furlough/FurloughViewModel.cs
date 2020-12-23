using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

            [Display(Name = "IsAllDay")] public bool IsAllDay { get; set; }

            IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
            {
                if (EndDate < StartDate || EndDate < DateTime.Now || StartDate < DateTime.Now)
                    yield return new ValidationResult("EndDate must be greater than StartDate", new[] {"EndDate"});
            }
        }
    }
}
