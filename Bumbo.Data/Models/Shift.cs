using System;
using System.ComponentModel.DataAnnotations.Schema;
using Bumbo.Data.Models.Common;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Data.Models
{
    public class Shift : BaseEntity
    {
        public int BranchId { get; set; }
        
        public int UserId { get; set; }
        
        public Department Department { get; set; }
        
        [Column(TypeName="date")]
        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }


        public User User { get; set; }
        
        public Branch Branch { get; set; }
        
        public WorkedShift WorkedShift { get; set; }
    }
}