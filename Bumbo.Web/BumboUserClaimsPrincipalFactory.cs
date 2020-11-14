using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Bumbo.Web
{
    public class BumboUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
    {
        private readonly ApplicationDbContext _dbContext;

        public BumboUserClaimsPrincipalFactory(ApplicationDbContext dbContext, UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
            _dbContext = dbContext;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User identityUser)
        {
            var identity = await base.GenerateClaimsAsync(identityUser);

            var branches = await _dbContext.Set<UserBranch>()
                .Where(userBranch => userBranch.UserId == identityUser.Id)
                .ToListAsync();

            identity.AddClaims(
                branches.Select(branch => new Claim("BranchDepartment", $"{branch.BranchId}.{branch.Department}"))
            );

            identity.AddClaims(
                branches
                    .Select(branch => branch.BranchId)
                    .Distinct()
                    .Select(branchId => new Claim("Branch", branchId.ToString(), ClaimValueTypes.Integer)
                    )
            );

            return identity;
        }
    }
}