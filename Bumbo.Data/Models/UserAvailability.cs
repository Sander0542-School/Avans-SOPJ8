using System;
using System.ComponentModel.DataAnnotations;
using Bumbo.Data.Models.Common;

namespace Bumbo.Data.Models
{
    public class UserAvailability : IEntity
    {
        public int UserId { get; set; }

        [Range(1, 7)]
        public int Day { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }


        public User User { get; set; }
    }
}