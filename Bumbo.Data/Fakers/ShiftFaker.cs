using System;
using Bogus;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Data.Fakers
{
    public class ShiftFaker : Faker<Shift>
    {
        public ShiftFaker() : base("nl")
        {
            RuleFor(o => o.Department, f => f.PickRandom<Department>());
            Rules((f, o) =>
            {
                var date = f.Date.Between(new DateTime(2020, 1, 1), new DateTime(2020, 12, 31));
                o.StartTime = date.Date.AddMinutes(f.Random.Int(32, 56) * 15);
                o.EndTime = o.StartTime.AddMinutes(f.Random.Int(12, 32) * 15);
            });
        }
    }
}