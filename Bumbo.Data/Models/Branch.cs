using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models.Common;

namespace Bumbo.Data.Models
{
    public class Branch : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        
        [StringLength(7, MinimumLength = 6)]
        [RegularExpression(@"^[1-9][0-9]{3} ?(?!sa|sd|ss)[a-zA-Z]{2}$")]
        [Required(ErrorMessage = "Zip Code is Required.")]
        public string ZipCode { get; set; }
        
        [StringLength(7, MinimumLength = 1)]
        [RegularExpression(@"^[1-9][0-9]{0,4}[a-z]{0,2}$")]
        [Required]
        public string HouseNumber { get; set; }
        
        
        public IList<Forecast> Forecasts { get; set; }
        
        public IList<BranchForecastStandard> ForecastStandards { get; set; }
        
        public IList<Shift> Shifts { get; set; }
    }
}