using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models.Common;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Data.Models
{
    public class ForecastStandard : IForecastStandard
    {
        [Key]
        public ForecastActivity Activity { get; set; }
        
        public int Value { get; set; }
        
        
        public IList<BranchForecastStandard> BranchForecastStandards { get; set; }
    }
}