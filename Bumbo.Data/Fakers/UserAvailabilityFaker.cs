using System;
using Bogus;
using Bumbo.Data.Models;

namespace Bumbo.Data.Fakers
{
    public class UserAvailabilityFaker : Faker<UserAvailability>
    {
        public UserAvailabilityFaker() : base("nl")
        {
            RuleFor(o => o.Day, f => f.Random.Int(1,7));
            RuleFor(o => o.StartTime, f => DateTime.Today.AddHours(f.Random.Int(32, 56)).TimeOfDay);
            RuleFor(o => o.EndTime, (f, o) => o.StartTime.Add(new TimeSpan(0, f.Random.Int(12, 32) * 15, 0)));
        }
        
        
    }
}