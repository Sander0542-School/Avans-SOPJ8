using Bumbo.Data;
using Bumbo.Web.Authorization.Handles;
using Bumbo.Web.Authorization.Requirements;
using Bumbo.Web.Models.Options;
using Microsoft.AspNetCore.Authorization;
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

        public static void AddPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Manager", policy => policy.RequireClaim("Manager"));
                
                options.AddPolicy("BranchManager", policy => policy.Requirements.Add(new BranchManagerRequirement()));
                options.AddPolicy("BranchEmployee", policy => policy.Requirements.Add(new BranchEmployeeRequirement()));
                options.AddPolicy("BranchDepartmentEmployee", policy => policy.Requirements.Add(new BranchDepartmentEmployeeRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, BranchManagerHandler>();
            services.AddSingleton<IAuthorizationHandler, BranchEmployeeHandler>();
            services.AddSingleton<IAuthorizationHandler, BranchDepartmentEmployeeHandler>();
        }
    }
}