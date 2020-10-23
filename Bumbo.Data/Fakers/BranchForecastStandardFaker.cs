using Bogus;
using Bumbo.Data.Models;

namespace Bumbo.Data.Fakers
{
    public class BranchForecastStandardFaker : Faker<BranchForecastStandard>
    {
        public BranchForecastStandardFaker() : base("nl")
        {
            RuleFor(o => o.Activity, f => f.Random.Word());
            RuleFor(o => o.Value, f => f.Random.Word());
        }
    }
}