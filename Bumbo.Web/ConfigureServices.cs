using Bumbo.Data;
using Bumbo.Web.Models.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bumbo.Web
{
    public static class ConfigureServices
    {
        public static void ConfigureRepositoryWrapper(this IServiceCollection services) => services.AddScoped<RepositoryWrapper>();

        public static void AddConfig(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<BumboOptions>(config.GetSection(BumboOptions.Bumbo));
        }
    }
}