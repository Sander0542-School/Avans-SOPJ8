using Bumbo.Data.Models.Common;

namespace Bumbo.Data.Models
{
    public class BranchForecastStandard : IEntity
    {
        public int BranchId { get; set; }
        
        public string Activity { get; set; }
        
        public string Value { get; set; }
        
        
        public Branch Branch { get; set; }
        
        public ForecastStandard ForecastStandard { get; set; }
    }
}