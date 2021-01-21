using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Bumbo.Web.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
namespace Bumbo.Web.Authorization.Handles
{
    public class YoungerThan18Handler : AuthorizationHandler<YoungerThan18Requirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, YoungerThan18Requirement requirement)
        {
            var birthdayTicks = long.Parse(context.User.FindFirst(ClaimTypes.DateOfBirth)?.Value ?? "0");
            var birthday = new DateTime(birthdayTicks);

            if (birthday >= DateTime.Today.AddYears(-18))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
