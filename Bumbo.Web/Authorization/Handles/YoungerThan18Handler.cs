using Bumbo.Data;
using Bumbo.Web.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bumbo.Web.Authorization.Handles
{
    public class YoungerThan18Handler : AuthorizationHandler<YoungerThan18Requirement>
    {
        private readonly RepositoryWrapper _wrapper;

        public YoungerThan18Handler(RepositoryWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, YoungerThan18Requirement requirement)
        {
            var userIdString = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null) return Task.CompletedTask;
            var userId = int.Parse(userIdString);

            var birthday = _wrapper.User.Get(u => u.Id == userId).Result.Birthday;
            if (birthday >= DateTime.Now.AddYears(-18))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}