using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Data.Fakers
{
    public static class FakeData
    {
        public static List<Branch> Branches = new List<Branch>();
        public static List<BranchForecastStandard> BranchForecastStandards = new List<BranchForecastStandard>();
        public static List<ClockSystemTag> ClockSystemTags = new List<ClockSystemTag>();
        public static List<Forecast> Forecasts = new List<Forecast>();
        public static List<ForecastStandard> ForecastStandards = new List<ForecastStandard>();
        public static List<Shift> Shifts = new List<Shift>();
        public static List<UserAdditionalWork> UserAdditionalWorks = new List<UserAdditionalWork>();
        public static List<UserAvailability> UserAvailabilities = new List<UserAvailability>();
        public static List<IdentityUser> Users = new List<IdentityUser>();
        public static List<WorkedShift> WorkedShifts = new List<WorkedShift>();

        public static void Init(int userCount, int branchCount)
        {
            var branchId = 0;
            var userId = 0;
            var shiftId = 0;

            ForecastStandards = new ForecastStandardFaker().Generate(30);

            var branchFaker = new BranchFaker()
                .CustomInstantiator(f => new Branch {Id = ++branchId})
                .RuleFor(o => o.ForecastStandards, (f, o) =>
                {
                    var forecastStandards = new Stack<ForecastStandard>(f.PickRandom(ForecastStandards, 10));
                    
                    BranchForecastStandards.AddRange(new BranchForecastStandardFaker()
                        .RuleFor(o2 => o2.BranchId, branchId)
                        .RuleFor(o2 => o2.Activity, forecastStandards.Pop().Activity)
                        .Generate(10)
                    );

                    return null;
                })
                .RuleFor(o => o.Forecasts, f =>
                {
                    var forecastDate = new DateTime(2020, 1, 1);

                    foreach (var department in Enum.GetValues(typeof(Department)))
                    {
                        Forecasts.AddRange(new ForecastFaker()
                            .RuleFor(o => o.BranchId, branchId)
                            .RuleFor(o => o.Department, department)
                            .RuleFor(o => o.Date, f =>
                            {
                                var date = forecastDate;
                                forecastDate = forecastDate.AddDays(1);

                                return date;
                            }).Generate(365));
                    }

                    return null;
                });

            Branches = branchFaker.Generate(branchCount);

            var userFaker = new UserFaker()
                .CustomInstantiator(f => new IdentityUser {Id = ++userId})
                .RuleFor(o => o.UserAvailabilities, f =>
                {
                    List<int> days = new List<int> {1, 2, 3, 4, 5, 6, 7};

                    UserAvailabilities.AddRange(new UserAvailabilityFaker()
                        .RuleFor(o => o.UserId, userId).RuleFor(o => o.Day, f2 =>
                        {
                            int day = f2.PickRandom(days);
                            days.Remove(day);

                            return day;
                        }).Generate(f.Random.Int(3, 7)));

                    return null;
                })
                .RuleFor(o => o.UserAdditionalWorks, f =>
                {
                    List<int> days = new List<int> {1, 2, 3, 4, 5, 6, 7};

                    UserAdditionalWorks.AddRange(new UserAdditionalWorkFaker()
                        .RuleFor(o => o.UserId, userId)
                        .RuleFor(o => o.Day, f2 =>
                        {
                            int day = f2.PickRandom(days);
                            days.Remove(day);

                            return day;
                        }).Generate(f.Random.Int(2, 5)));

                    return null;
                })
                .RuleFor(o => o.ClockSystemTags, f =>
                {
                    ClockSystemTags.AddRange(new ClockSystemTagFaker()
                        .RuleFor(o => o.UserId, userId)
                        .Generate(f.Random.Int(1, 3)));

                    return null;
                }).RuleFor(o => o.Shifts, f =>
                {
                    int branchId = f.Random.Int(1, branchCount);
                    var shiftDate = new DateTime(2020, 1, 1);

                    IEnumerable<Shift> shifts = new ShiftFaker()
                        .CustomInstantiator(f => new Shift {Id = ++shiftId})
                        .RuleFor(o => o.UserId, userId)
                        .RuleFor(o => o.BranchId, branchId)
                        .Rules((f2, o) =>
                        {
                            shiftDate = shiftDate.AddDays(f2.Random.Int(1, 5));

                            o.StartTime = shiftDate.Date.AddMinutes(f.Random.Int(32, 56) * 15);
                            o.EndTime = o.StartTime.AddMinutes(f.Random.Int(12, 32) * 15);
                        })
                        .GenerateForever();

                    foreach (var shift in shifts)
                    {
                        if (shift.EndTime > new DateTime(2020, 12, 31)) break;
                        
                        Shifts.Add(shift);
                        
                        WorkedShifts.Add(new WorkedShift
                        {
                            ShiftId = shift.Id,
                            Sick = f.Random.Bool(0.1f),
                            StartTime = shift.StartTime,
                            EndTime = shift.EndTime.AddMinutes(f.Random.Int(0, 6) * 15)
                        });
                    }

                    return null;
                });

            Users = userFaker.Generate(userCount);
        }
    }
}