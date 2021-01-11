﻿using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Bumbo.Web
{
    public class BumboUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
    {
        private readonly ApplicationDbContext _dbContext;

        public BumboUserClaimsPrincipalFactory(ApplicationDbContext dbContext, UserManager<User> userManager, RoleManager<Role> roleManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
        {
            _dbContext = dbContext;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User identityUser)
        {
            var identity = await base.GenerateClaimsAsync(identityUser);

            var userBranches = await _dbContext.Set<UserBranch>()
                .Where(userBranch => userBranch.UserId == identityUser.Id)
                .ToListAsync();

            var managerBranches = await _dbContext.Set<BranchManager>()
                .Where(branchManager => branchManager.UserId == identityUser.Id)
                .Select(branchManager => branchManager.BranchId)
                .Distinct()
                .ToListAsync();

            identity.AddClaims(
                userBranches.Select(branch => new Claim("BranchDepartment", $"{branch.BranchId}.{branch.Department}"))
            );

            identity.AddClaims(
                managerBranches.Select(branchId => new Claim("Manager", branchId.ToString(), ClaimValueTypes.Integer))
            );

            identity.AddClaims(
                userBranches
                    .Select(branch => branch.BranchId)
                    .Concat(managerBranches)
                    .Distinct()
                    .Select(branchId => new Claim("Branch", branchId.ToString(), ClaimValueTypes.Integer))
            );

            identity.AddClaim(new Claim(ClaimTypes.DateOfBirth, identityUser.Birthday.Ticks.ToString()));

            return identity;
        }
    }
}
