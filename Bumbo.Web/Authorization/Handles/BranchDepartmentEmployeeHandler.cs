using System;
using System.Threading.Tasks;
using Bumbo.Data.Models.Enums;
using Bumbo.Web.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Bumbo.Web.Authorization.Handles
{
    public class BranchDepartmentEmployeeHandler : AuthorizationHandler<BranchDepartmentEmployeeRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BranchDepartmentEmployeeHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BranchDepartmentEmployeeRequirement requirement)
        {
            var branchId = _httpContextAccessor.HttpContext?.GetRouteValue("branchId")?.ToString() ?? "0";
            var department = Enum.Parse<Department>(_httpContextAccessor.HttpContext?.GetRouteValue("department")?.ToString() ?? "0", true);

            if (context.User.HasClaim("BranchDepartment", $"{branchId}.{department}"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
