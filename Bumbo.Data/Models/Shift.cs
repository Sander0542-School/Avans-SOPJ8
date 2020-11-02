using System;
using Bumbo.Data.Models.Common;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Data.Models
{
    public class Shift : BaseEntity
    {
        public int BranchId { get; set; }
        
        public int UserId { get; set; }
        
        public Department Department { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }


        public IdentityUser User { get; set; }
        
        public Branch Branch { get; set; }
        
        public WorkedShift WorkedShift { get; set; }
    }
}