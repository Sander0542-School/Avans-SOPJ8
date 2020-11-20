using Bumbo.Data.Models.Common;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Data.Models
{
    public class WeekSchedule : IEntity
    {
        public int BranchId { get; set; }
        
        public int Year { get; set; }
        
        public int Week { get; set; }
        
        public Department Department { get; set; }
        
        public bool Confirmed { get; set; }
        
        
        public Branch Branch { get; set; }
    }
}