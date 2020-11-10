using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models.Common;

namespace Bumbo.Data.Models
{
    public class ForecastStandard : IEntity
    {
        [Key]
        public string Activity { get; set; }
        
        public string Value { get; set; }
        
        
        public IList<BranchForecastStandard> BranchForecastStandards { get; set; }
    }
}