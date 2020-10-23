using System;
using Bogus;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Data.Fakers
{
    public class ForecastFaker : Faker<Forecast>
    {
        public ForecastFaker() : base("nl")
        {
            RuleFor(o => o.WorkingHours, f => f.Random.Double() * 100);
        }
    }
}