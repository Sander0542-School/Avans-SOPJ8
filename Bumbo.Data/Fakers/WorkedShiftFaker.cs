using Bogus;
using Bumbo.Data.Models;

namespace Bumbo.Data.Fakers
{
    public class WorkedShiftFaker : Faker<WorkedShift>
    {
        public WorkedShiftFaker() : base("nl")
        {
            RuleFor(o => o.Sick, f => f.Random.Bool(0.1f));
        }
    }
}