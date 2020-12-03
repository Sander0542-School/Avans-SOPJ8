using System;
using Bumbo.Data.Models.Common;

namespace Bumbo.Data.Models
{
    public class UserAdditionalWork : IEntity
    {
        public int UserId { get; set; }

        public DayOfWeek Day { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public User User { get; set; }
    }
}