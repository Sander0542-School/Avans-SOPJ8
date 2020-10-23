using Bogus;
using Bumbo.Data.Models;

namespace Bumbo.Data.Fakers
{
    public class ForecastStandardFaker : Faker<ForecastStandard>
    {
        public ForecastStandardFaker() : base("nl")
        {
            RuleFor(o => o.Activity, f => f.Random.Word());
            RuleFor(o => o.Value, f => f.Random.Word());
        }
    }
}