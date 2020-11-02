using System;
using System.ComponentModel.DataAnnotations;

namespace Bumbo.Data.Models
{
    public class UserAvailability
    {
        public int UserId { get; set; }

        [Range(1, 7)]
        public int Day { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }


        public IdentityUser User { get; set; }
    }
}