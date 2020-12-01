using System.Collections.Generic;
using Bumbo.Data.Models.Common;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Data.Models
{
    public class BranchSchedule : IEntity
    {
        public int BranchId { get; set; }
        
        public Department Department { get; set; }
        
        public int Year { get; set; }
        
        public int Week { get; set; }
        
        public bool Confirmed { get; set; }
        
        
        public Branch Branch { get; set; }
        
        public List<Shift> Shifts { get; set; }
    }
}