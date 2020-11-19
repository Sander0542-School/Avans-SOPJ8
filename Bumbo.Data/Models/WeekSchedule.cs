using Bumbo.Data.Models.Common;

namespace Bumbo.Data.Models
{
    public class WeekSchedule : IEntity
    {
        public int Year { get; set; }
        
        public int Week { get; set; }
        
        public bool Confirmed { get; set; }
    }
}