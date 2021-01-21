using System;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models.Common;
namespace Bumbo.Data.Models
{
    public class UserAvailability : IEntity
    {
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Day")]
        public DayOfWeek Day { get; set; }

        [Required]
        [Display(Name = "Starttime")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Display(Name = "Endtime")]
        public TimeSpan EndTime { get; set; }

        public User User { get; set; }
    }
}
