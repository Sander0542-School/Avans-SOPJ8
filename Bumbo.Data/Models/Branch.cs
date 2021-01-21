using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models.Common;
using Bumbo.Data.Models.Validators;
namespace Bumbo.Data.Models
{
    public class Branch : BaseEntity
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [StringLength(7, MinimumLength = 6)]
        [ZipCode]
        [Required(ErrorMessage = "Zip Code is Required.")]
        [Display(Name = "Zip code")]
        public string ZipCode { get; set; }

        [StringLength(7, MinimumLength = 1)]
        [BuildingNumber]
        [Required]
        [Display(Name = "House number")]
        public string HouseNumber { get; set; }


        public IList<Forecast> Forecasts { get; set; }

        public IList<BranchForecastStandard> ForecastStandards { get; set; }

        public IList<BranchSchedule> Schedules { get; set; }

        public IList<UserBranch> Users { get; set; }

        public IList<BranchManager> Managers { get; set; }
    }
}
