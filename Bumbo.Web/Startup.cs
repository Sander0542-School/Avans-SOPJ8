using System;
using System.Collections.Generic;
using System.Globalization;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Data.Seeder;
using Bumbo.Logic.Services.Weather;
using Bumbo.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
namespace Bumbo.Web
{
    public class Startup
    {
        private readonly bool _isTestEnv;
        private SqliteConnection _sqLiteTestConnection;
        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            _isTestEnv = env.IsTesting();

            if (_isTestEnv)
            {
                Console.WriteLine(@"Running in test mode");
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
            });

            services.AddDatabaseDeveloperPageExceptionFilter();

            if (_isTestEnv)
            {
                Console.WriteLine(@"Using local SQLite database");
                _sqLiteTestConnection = new SqliteConnection("Data Source=:memory:;Mode=Memory;Cache=Shared");
                // This connection ensures the database is not deleted
                _sqLiteTestConnection.Open();
            }

            services.AddDbContext<ApplicationDbContext>(
            options =>
            {
                if (_isTestEnv)
                {
                    options.UseSqlite(_sqLiteTestConnection)
                        .EnableSensitiveDataLogging();
                }
                else
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection"));
                }
            }, ServiceLifetime.Transient
            );

            services
                .AddDefaultIdentity<User>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = !_isTestEnv;
                    options.User.RequireUniqueEmail = true;
                })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddClaimsPrincipalFactory<BumboUserClaimsPrincipalFactory>();

            services.ConfigureRepositoryWrapper();

            services.AddConfig(Configuration);

            services.AddPolicies();

            // Localization configuration
            // Could add more cultures later like german 
            services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
            services.Configure<RequestLocalizationOptions>(
            opt =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new("nl-NL"), new("en-US")
                };
                opt.DefaultRequestCulture = new RequestCulture("nl-NL");
                opt.SupportedCultures = supportedCultures;
                opt.SupportedUICultures = supportedCultures;
            });

            services.AddScoped<IOpenWeatherMapService, OpenWeatherMapService>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context)
        {
            if (env.IsDevelopment() || env.IsTesting())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (env.IsTesting())
            {
                context.Database.EnsureCreated();
            }

            Web.ConfigureServices.SeedRoles(app.ApplicationServices).Wait();

            if (env.IsTesting())
            {
                context.Database.EnsureCreated();

                var seeder = new TestDataSeeder();

                #region UserData

                var userManager = app.ApplicationServices.GetService<UserManager<User>>();
                foreach (var user in seeder.Users)
                {
                    userManager.CreateAsync(user, "Pass1234!").Wait();

                    if (user.Id == TestDataSeeder.SuperId)
                    {
                        userManager.AddToRoleAsync(user, "SuperUser");
                    }
                }

                context.UserAvailabilities.AddRange(seeder.UserAvailabilities);
                context.Set<UserContract>().AddRange(seeder.UserContracts);
                context.ClockSystemTags.AddRange(seeder.ClockSystemTags);

                #endregion

                #region BranchData

                context.Branches.AddRange(seeder.Branches);
                context.Set<BranchManager>().AddRange(seeder.BranchManagers);
                context.Set<UserBranch>().AddRange(seeder.BranchEmployees);

                context.Set<BranchSchedule>().AddRange(seeder.Shifts);

                #endregion

                context.SaveChanges();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRequestLocalization(app.ApplicationServices
                .GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            // Could add more cultures later like german 
            //var supportedCultures = new[] {"en", "nl"};
            //var localizationOptions = new RequestLocalizationOptions()
            //    .SetDefaultCulture(supportedCultures[1])
            //    .AddSupportedCultures(supportedCultures)
            //    .AddSupportedUICultures(supportedCultures);

            //app.UseRequestLocalization(localizationOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
