using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models;
using Bumbo.Web.Controllers;
using Microsoft.Extensions.Localization;

namespace Bumbo.Web.Models.Users
{
    public class ContractViewModel : IValidatableObject
    {
        [Required]
        [Display(Name = "User Id")]
        public int UserId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Function")]
        public string Function { get; set; }

        [Required]
        [Display(Name = "Scale")]
        public int Scale { get; set; }

        public User User { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            IStringLocalizer _Localizer = (IStringLocalizer)validationContext.GetService(typeof(IStringLocalizer<ContractViewModel>));
            if (StartDate > EndDate)
            {
                yield return new ValidationResult(_Localizer["The start date cannot be after the end date"]);
            }
        }
    }


}

