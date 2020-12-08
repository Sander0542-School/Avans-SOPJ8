using System;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models.Common;

namespace Bumbo.Data.Models
{
    public class WorkedShift : IEntity
    {
        [Key]
        public int ShiftId { get; set; }
        
        public bool Sick { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }
        
        public Shift Shift { get; set; }
    }
}