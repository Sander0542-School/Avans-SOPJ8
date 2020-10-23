using Bogus;
using Microsoft.AspNetCore.Identity;
using IdentityUser = Bumbo.Data.Models.IdentityUser;

namespace Bumbo.Data.Fakers
{
    public class UserFaker : Faker<IdentityUser>
    {
        public UserFaker() : base("nl")
        {
            RuleFor(o => o.FirstName, f => f.Person.FirstName);
            // RuleFor(o => o.MiddleName, f => f.Name.Insertion()); Bogus doesn't support this yet
            RuleFor(o => o.LastName, f => f.Person.LastName);
            RuleFor(o => o.Email, f => f.Person.Email);
            RuleFor(o => o.NormalizedEmail, f => f.Person.Email.ToUpper());
            RuleFor(o => o.UserName, f => f.Person.UserName);
            RuleFor(o => o.NormalizedUserName, f => f.Person.UserName.ToUpper());
            RuleFor(o => o.PhoneNumber, f => f.Person.Phone);
            RuleFor(o => o.Birthday, f => f.Person.DateOfBirth);

            RuleFor(o => o.EmailConfirmed, f => f.Random.Bool(0.9f));
            RuleFor(o => o.PhoneNumberConfirmed, f => f.Random.Bool(0.9f));
            
            RuleFor(o => o.ZipCode, f => f.Address.ZipCode());
            RuleFor(o => o.HouseNumber, f => f.Address.BuildingNumber());

            FinishWith((f, o) =>
            {
                var hasher = new PasswordHasher<IdentityUser>();
                o.PasswordHash = hasher.HashPassword(o, f.Internet.Password());
            });
        }
    }
}