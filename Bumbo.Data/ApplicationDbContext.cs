using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bumbo.Data.Fakers;
using Bumbo.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Bumbo.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        
        public DbSet<Branch> Branches { get; set; }
        
        public DbSet<ClockSystemTag> ClockSystemTags { get; set; }

        public DbSet<WorkedShift> WorkedShifts { get; set; }
        
        public DbSet<Shift> Shifts { get; set; }
        
        public DbSet<UserAvailability> UserAvailabilities { get; set; }
        
        public DbSet<UserAdditionalWork> UserAdditionalWorks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            FakeData.Init(500, 10);

            #region Identity
            
            builder.Entity<User>(b =>
            {
                b.ToTable("Users");
                
                b.HasData(FakeData.Users);
            });

            builder.Entity<IdentityUserClaim<int>>(b =>
            {
                b.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<int>>(b =>
            {
                b.ToTable("UserLogins");
            });

            builder.Entity<IdentityUserToken<int>>(b =>
            {
                b.ToTable("UserTokens");
            });

            builder.Entity<IdentityRole<int>>(b =>
            {
                b.ToTable("Roles");
            });

            builder.Entity<IdentityRoleClaim<int>>(b =>
            {
                b.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserRole<int>>(b =>
            {
                b.ToTable("UserRoles");
            });
            
            #endregion

            #region Users

            builder.Entity<UserAdditionalWork>(b =>
            {
                b.HasKey(additionalWork => new {additionalWork.UserId, additionalWork.Day});
                
                b.HasData(FakeData.UserAdditionalWorks);
            });
            
            builder.Entity<UserAvailability>(b =>
            {
                b.HasKey(availability => new {availability.UserId, availability.Day});
                
                b.HasData(FakeData.UserAvailabilities);
            });

            #endregion

            #region Branches

            builder.Entity<Branch>(b =>
            {
                b.HasData(FakeData.Branches);
            });

            #endregion

            #region Shifts

            builder.Entity<Shift>(b =>
            {
                b.HasData(FakeData.Shifts);
            });

            builder.Entity<WorkedShift>(b =>
            {
                b.Property(workedShift => workedShift.Sick).HasDefaultValue(false);
                
                b.HasData(FakeData.WorkedShifts);
            });

            #endregion

            #region Forecast
            
            builder.Entity<Forecast>(b =>
            {
                b.HasKey(forecast => new {forecast.BranchId, forecast.Date, forecast.Department});
                
                b.HasData(FakeData.Forecasts);
            });

            builder.Entity<ForecastStandard>(b =>
            {
                b.HasData(FakeData.ForecastStandards);
            });

            builder.Entity<BranchForecastStandard>(b =>
            {
                b.HasKey(branchForecastStandard => new {branchForecastStandard.BranchId, branchForecastStandard.Activity});

                b.HasOne(branchForecastStandard => branchForecastStandard.ForecastStandard)
                    .WithMany(forecastStandard => forecastStandard.BranchForecastStandards)
                    /* .HasForeignKey(branchForecastStandard => branchForecastStandard.Activity)*/;
                
                b.HasData(FakeData.BranchForecastStandards);
            });

            #endregion

            #region ClockSystem

            builder.Entity<ClockSystemTag>(b =>
            {
                b.HasData(FakeData.ClockSystemTags);
            });

            #endregion
        }
    }

    public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/../Bumbo.Web/appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DatabaseConnection");
            builder.UseSqlServer(connectionString);
            return new ApplicationDbContext(builder.Options);
        }
    }
}