using System;
using System.Collections.Generic;
using System.Globalization;
using Bumbo.Data;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            _isTestEnv = env.IsEnvironment("Testing");

            if(_isTestEnv) 
                Console.WriteLine("Running in test mode");
        }

        public IConfiguration Configuration { get; }
        private readonly bool _isTestEnv;
        private SqliteConnection _sqLiteTestConnection;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseDeveloperPageExceptionFilter();

            if (_isTestEnv)
            {
                Console.WriteLine("Using local SQLite database");
                _sqLiteTestConnection = new SqliteConnection("Data Source=:memory:;Mode=Memory;Cache=Shared");
                // This connection ensures the database is not deleted
                _sqLiteTestConnection.Open();
            }

            services.AddDbContext<ApplicationDbContext>(
                options =>
                {
                    if (_isTestEnv)
                        options.UseSqlite(_sqLiteTestConnection);
                    else
                        options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection"));
                }
            );

            services
                .AddDefaultIdentity<User>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.User.RequireUniqueEmail = true;
                })
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
                        new CultureInfo("nl-NL"),
                        new CultureInfo("en-US")
                    };
                    opt.DefaultRequestCulture = new RequestCulture("nl-NL");
                    opt.SupportedCultures = supportedCultures;
                    opt.SupportedUICultures = supportedCultures;
                });

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context)
        {
            if (env.IsDevelopment() || _isTestEnv)
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

            if (_isTestEnv)
            {
                context.Database.EnsureCreated();
                // Todo: Add database seeder method
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

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
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}