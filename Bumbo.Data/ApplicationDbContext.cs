using System;
using System.IO;
using System.Linq;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Bumbo.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
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

        public DbSet<ForecastStandard> ForecastStandard { get; set; }

        public DbSet<Furlough> Furloughs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Identity

            builder.Entity<User>(b =>
            {
                b.ToTable("Users");
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

            builder.Entity<Role>(b =>
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

            builder.Entity<UserContract>(b =>
            {
                b.ToTable("Contracts");
            });

            builder.Entity<UserAdditionalWork>(b =>
            {
                b.HasKey(additionalWork => new { additionalWork.UserId, additionalWork.Day });
            });

            builder.Entity<UserAvailability>(b =>
            {
                b.HasKey(availability => new { availability.UserId, availability.Day });
            });

            builder.Entity<UserBranch>(b =>
            {
                b.HasKey(userBranch => new { userBranch.UserId, userBranch.BranchId, userBranch.Department });
            });

            #endregion

            #region Branches

            builder.Entity<Branch>(b =>
            {

            });

            builder.Entity<BranchManager>(b =>
            {
                b.HasKey(branchManager => new { branchManager.UserId, branchManager.BranchId });

                b.ToTable("BranchManagers");
            });

            builder.Entity<BranchSchedule>(b =>
            {
                b.HasIndex(branchSchedule => new { branchSchedule.BranchId, branchSchedule.Year, branchSchedule.Week, branchSchedule.Department }).IsUnique();

                b.ToTable("BranchSchedules");
            });

            #endregion

            #region Shifts

            builder.Entity<Shift>(b =>
            {
                b.HasIndex(shift => new { shift.UserId, shift.ScheduleId, shift.Date }).IsUnique();
            });

            builder.Entity<WorkedShift>(b =>
            {
                b.Property(workedShift => workedShift.Sick).HasDefaultValue(false);
            });

            #endregion

            #region Forecast

            builder.Entity<Forecast>(b =>
            {
                b.HasKey(forecast => new { forecast.BranchId, forecast.Date, forecast.Department });
            });

            builder.Entity<ForecastStandard>(b =>
            {
                // Loops through enum and generates a forecast standard for each item with value 10
                b.HasData(Enum.GetValues<ForecastActivity>().Select(activity =>
                    new ForecastStandard { Value = 10, Activity = activity })
                );
            });

            builder.Entity<BranchForecastStandard>(b =>
            {
                b.HasKey(branchForecastStandard => new { branchForecastStandard.BranchId, branchForecastStandard.Activity });

                b.HasOne(branchForecastStandard => branchForecastStandard.ForecastStandard)
                    .WithMany(forecastStandard => forecastStandard.BranchForecastStandards)
                    /* .HasForeignKey(branchForecastStandard => branchForecastStandard.Activity)*/;
            });

            #endregion

            #region ClockSystem

            builder.Entity<ClockSystemTag>(b =>
            {

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
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/../Bumbo.Web/appsettings.Development.json")
                .Build();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DatabaseConnection");
            builder.UseSqlServer(connectionString);
            return new ApplicationDbContext(builder.Options);
        }
    }
}
