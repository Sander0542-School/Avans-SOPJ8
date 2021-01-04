using Bumbo.Data.Models.Common;
using Microsoft.AspNetCore.Identity;

namespace Bumbo.Data.Models
{
    public class Role : IdentityRole<int>, IEntity
    {
        public Role(string role) : base(role)
        {
        }

        public Role()
        {
        }
    }
}
