using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bumbo.Data.Models
{
    public class ForecastStandard
    {
        [Key]
        public string Activity { get; set; }
        
        public string Value { get; set; }
        
        
        public IList<BranchForecastStandard> BranchForecastStandards { get; set; }
    }
}