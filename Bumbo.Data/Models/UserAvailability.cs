using Bumbo.Data.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace Bumbo.Data.Models
{
    public class UserAvailability : IEntity
    {
        public int UserId { get; set; }

        [Range(1, 7)]
        public DayOfWeek Day { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }


        public User User { get; set; }
    }
}