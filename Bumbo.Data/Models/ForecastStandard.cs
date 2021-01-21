using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models.Common;
using Bumbo.Data.Models.Enums;
namespace Bumbo.Data.Models
{
    public class ForecastStandard : IForecastStandard
    {


        public IList<BranchForecastStandard> BranchForecastStandards { get; set; }
        [Key]
        public ForecastActivity Activity { get; set; }

        [Range(1, 500)]
        public int Value { get; set; }
    }
}
