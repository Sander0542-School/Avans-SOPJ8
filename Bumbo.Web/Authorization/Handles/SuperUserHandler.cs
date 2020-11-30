using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bumbo.Data.Models;
using Bumbo.Web.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Bumbo.Web.Authorization.Handles
{
    public class SuperUserHandler : AuthorizationHandler<SuperUserRequirement>
    {
        private readonly UserManager<User> _userManager;

        public SuperUserHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SuperUserRequirement requirement)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Task.CompletedTask;
            var userIdsInSuperUser = _userManager.GetUsersInRoleAsync("SuperUser").Result.Select(u => u.Id);


            if (userIdsInSuperUser.Contains(int.Parse(userId)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}