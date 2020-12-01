using Bumbo.Data.Models.Common;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Data.Models
{
    public class BranchForecastStandard : IForecastStandard
    {
        public int BranchId { get; set; }

        public ForecastActivity Activity { get; set; }

        public int Value { get; set; }


        public Branch Branch { get; set; }

        public ForecastStandard ForecastStandard { get; set; }
    }
}