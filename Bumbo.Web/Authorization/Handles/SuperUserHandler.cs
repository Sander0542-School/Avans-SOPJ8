using System.Threading.Tasks;
using Bumbo.Web.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Bumbo.Web.Authorization.Handles
{
    public class SuperUserHandler : AuthorizationHandler<BranchManagerRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BranchManagerRequirement requirement)
        {
            // TODO: Add proper role verification
            // if (context.User.HasClaim("SuperUser", "1"))
            // {
            //     context.Succeed(requirement);
            // }

            return Task.CompletedTask;
        }
    }
}