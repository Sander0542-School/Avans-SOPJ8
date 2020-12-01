using Bumbo.Web.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace Bumbo.Web.Authorization.Handles
{
    public class BranchManagerHandler : AuthorizationHandler<BranchManagerRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BranchManagerHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BranchManagerRequirement requirement)
        {
            var branchId = _httpContextAccessor.HttpContext?.GetRouteValue("branchId")?.ToString() ?? "0";

            if (context.User.HasClaim("Manager", branchId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}