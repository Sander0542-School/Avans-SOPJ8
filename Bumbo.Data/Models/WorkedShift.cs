using System;
using Bumbo.Data.Models.Common;

namespace Bumbo.Data.Models
{
    public class WorkedShift : BaseEntity
    {
        public int UserId { get; set; }
        
        public int ScheduleId { get; set; }
        
        public bool Sick { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }


        public IdentityUser User { get; set; }
        
        public Shift Shift { get; set; }
    }
}