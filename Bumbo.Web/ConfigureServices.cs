using System;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Web.Authorization.Handles;
using Bumbo.Web.Authorization.Requirements;
using Bumbo.Web.Models.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bumbo.Web
{
    public static class ConfigureServices
    {
        public static void ConfigureRepositoryWrapper(this IServiceCollection services) => services.AddTransient<RepositoryWrapper>();

        public static void AddConfig(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<BumboOptions>(config.GetSection(BumboOptions.Bumbo));
        }

        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roles = new[] { "SuperUser" };
            using var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new Role(role));
                }
            }
        }

        public static void AddPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SuperUser", policy => policy.RequireRole("SuperUser"));
                options.AddPolicy("YoungerThan18", policy => policy.Requirements.Add(new YoungerThan18Requirement()));

                options.AddPolicy("Manager", policy => policy.RequireClaim("Manager"));

                options.AddPolicy("BranchManager", policy => policy.Requirements.Add(new BranchManagerRequirement()));
                options.AddPolicy("BranchEmployee", policy => policy.Requirements.Add(new BranchEmployeeRequirement()));
                options.AddPolicy("BranchDepartmentEmployee", policy => policy.Requirements.Add(new BranchDepartmentEmployeeRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, BranchManagerHandler>();
            services.AddSingleton<IAuthorizationHandler, BranchEmployeeHandler>();
            services.AddSingleton<IAuthorizationHandler, BranchDepartmentEmployeeHandler>();
            services.AddScoped<IAuthorizationHandler, YoungerThan18Handler>(); // This service is scoped because it relies on a scoped repository wrapper
        }
    }
}
