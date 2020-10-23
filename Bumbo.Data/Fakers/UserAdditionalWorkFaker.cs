using Bogus;
using Bumbo.Data.Models;

namespace Bumbo.Data.Fakers
{
    public class UserAdditionalWorkFaker : Faker<UserAdditionalWork>
    {
        public UserAdditionalWorkFaker() : base("nl")
        {
            RuleFor(o => o.Day, f => f.Random.Int(1,7));
            RuleFor(o => o.Hours, f => f.Random.Int(12,32) / 4);
        }
    }
}