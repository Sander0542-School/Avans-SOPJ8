namespace Bumbo.Data.Models
{
    public class BranchForecastStandard
    {
        public int BranchId { get; set; }
        
        public string Activity { get; set; }
        
        public string Value { get; set; }
        
        
        public Branch Branch { get; set; }
        
        public ForecastStandard ForecastStandard { get; set; }
    }
}