using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bumbo.Data.Models.Common;

namespace Bumbo.Data.Models
{
    public class WorkedShift
    {
        [Key]
        public int ShiftId { get; set; }
        
        public bool Sick { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        
        public Shift Shift { get; set; }
    }
}