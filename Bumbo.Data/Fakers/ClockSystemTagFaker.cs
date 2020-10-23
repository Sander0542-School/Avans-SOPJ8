using Bogus;
using Bumbo.Data.Models;

namespace Bumbo.Data.Fakers
{
    public class ClockSystemTagFaker : Faker<ClockSystemTag>
    {
        public ClockSystemTagFaker() : base("nl")
        {
            RuleFor(o => o.SerialNumber, f => f.Internet.Mac());
        }
    }
}