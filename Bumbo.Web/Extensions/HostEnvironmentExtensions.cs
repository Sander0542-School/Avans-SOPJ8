using Microsoft.Extensions.Hosting;

namespace Bumbo.Web.Extensions
{
    public static class HostEnvironmentExtensions
    {
        public static bool IsTesting(this IHostEnvironment environment)
        {
            return environment.IsEnvironment("Testing");
        }
    }
}