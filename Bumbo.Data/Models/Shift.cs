using System;
using System.ComponentModel.DataAnnotations.Schema;
using Bumbo.Data.Models.Common;
namespace Bumbo.Data.Models
{
    public class Shift : BaseEntity
    {
        public int ScheduleId { get; set; }

        public int UserId { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public bool Offered { get; set; }
        public bool OfferedCrossBranch { get; set; }

        public User User { get; set; }

        public BranchSchedule Schedule { get; set; }

        public WorkedShift WorkedShift { get; set; }
    }
}
