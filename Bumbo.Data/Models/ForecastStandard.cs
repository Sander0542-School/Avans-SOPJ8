using Bumbo.Data.Models.Common;
using Bumbo.Data.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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