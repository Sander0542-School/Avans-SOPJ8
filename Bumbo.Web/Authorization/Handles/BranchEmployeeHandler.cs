using System;
using System.Threading.Tasks;
using Bumbo.Web.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Bumbo.Web.Authorization.Handles
{
    public class BranchEmployeeHandler : AuthorizationHandler<BranchEmployeeRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BranchEmployeeHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BranchEmployeeRequirement requirement)
        {
            var branchId = _httpContextAccessor.HttpContext?.GetRouteValue("branchId")?.ToString() ?? "0";

            if (context.User.HasClaim("Branch", branchId))
            {
                context.Succeed(requirement);
            }
            
            return Task.CompletedTask;
        }
    }
}