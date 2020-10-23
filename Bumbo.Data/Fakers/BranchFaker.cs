using Bogus;
using Bumbo.Data.Models;

namespace Bumbo.Data.Fakers
{
    public class BranchFaker : Faker<Branch>
    {
        public BranchFaker() : base("nl")
        {
            RuleFor(o => o.Name, f => f.Address.City());
            RuleFor(o => o.ZipCode, f => f.Address.ZipCode());
            RuleFor(o => o.HouseNumber, f => f.Address.BuildingNumber());
        }
    }
}