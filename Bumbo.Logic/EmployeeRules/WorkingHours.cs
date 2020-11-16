using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Bumbo.Data.Models;
using Bumbo.Logic.Utils;

namespace Bumbo.Logic.EmployeeRules
{
    public class WorkingHours
    {
        public static Dictionary<Shift, List<string>> ValidateWeek(User user, int year, int week)
        {
            ValidateUserProperties(user);

            var wrongShifts = new Dictionary<Shift, List<string>>();

            var beginOfWeek = ISOWeek.ToDateTime(year, week, DayOfWeek.Monday);

            var shifts = user.Shifts.Where(shift => shift.StartTime >= beginOfWeek).Where(shift => shift.StartTime < beginOfWeek.AddDays(7)).ToList();

            var totalDuration = new TimeSpan(shifts
                .Select(shift => (shift.EndTime - shift.StartTime))
                .Sum(time => time.Ticks));

            var maxWeekHours = MaxHoursPerWeek(user, year, week);
            var tooManyHours = totalDuration > maxWeekHours;

            var maxDays = MaxDayPerAWeek(user);
            var tooManyDays = shifts.Select(shift => shift.StartTime.Day).Distinct().Count() > maxDays;

            foreach (var shift in shifts)
            {
                var notifications = new List<string>();

                if (tooManyHours)
                    notifications.Add(String.Format("Deze medewerker mag niet meer dan {0:hh\\:mm} uur per week werken", maxWeekHours));

                if (tooManyDays)
                    notifications.Add(String.Format("Deze medewerker mag niet meer dan {0} dagen per week werken", maxDays));

                notifications.AddRange(ValidateShift(user, shift));

                if (notifications.Count > 0)
                    wrongShifts.Add(shift, notifications);
            }

            return wrongShifts;
        }

        public static IEnumerable<string> ValidateShift(User user, Shift shift)
        {
            var notifications = new List<string>();

            var age = UserUtil.GetAge(user);
            var shiftDuration = shift.EndTime - shift.StartTime;
            var maxHours = MaxHoursPerDay(user, shift.StartTime.Day);

            var availability = user.UserAvailabilities.FirstOrDefault(userAvailability => userAvailability.Day == shift.StartTime.Day);

            if (age < 16 && shift.EndTime.TimeOfDay > new TimeSpan(19, 0, 0))
                notifications.Add("Deze medewerker mag niet later dan 19:00 uur werken");

            if (shiftDuration > maxHours)
                notifications.Add($"Deze medewerker mag deze dag niet meer dan {maxHours:hh\\:mm} uur werken");

            if (availability == null)
                notifications.Add("Deze medewerker wil deze dag niet werken");
            else if (!ShiftBetweenAvailableTime(shift, availability))
                notifications.Add($"Deze medewerker wil deze dag tussen {availability.StartTime:hh\\:mm} en {availability.EndTime:hh\\:mm} werken");

            return notifications;
        }

        public static bool ShiftBetweenAvailableTime(Shift shift, UserAvailability availability)
        {
            if (shift.StartTime.TimeOfDay < availability.StartTime)
                return false;
            if (shift.EndTime.TimeOfDay > availability.EndTime)
                return false;

            return true;
        }

        public static TimeSpan MaxHoursPerWeek(User user, int year, int week)
        {
            var age = UserUtil.GetAge(user, ISOWeek.ToDateTime(year, week, DayOfWeek.Monday));

            var maxHours = 60;

            if (age < 16)
            {
                //TODO Check for School week: return 12;

                maxHours = 40;
            }

            if (age < 18) maxHours = 40;

            return new TimeSpan(maxHours, 0, 0);
        }

        public static TimeSpan MaxHoursPerDay(User user, int day)
        {
            var age = UserUtil.GetAge(user);

            if (age < 18)
            {
                var otherWorkHours = user.UserAdditionalWorks.FirstOrDefault(work => work.Day == day)?.Hours ?? 0;
                var maxHours = 9;

                if (age < 16) maxHours = 8;

                return new TimeSpan(0, (int) ((otherWorkHours > maxHours ? 0 : maxHours - otherWorkHours) * 60), 0);
            }

            return new TimeSpan(12, 0, 0);
        }

        public static int MaxDayPerAWeek(User user) => UserUtil.GetAge(user) < 16 ? 5 : 7;

        public static void ValidateUserProperties(User user)
        {
            if (user.Shifts == null)
                throw new ArgumentException("The user does not contain a list of shifts");

            if (user.UserAvailabilities == null)
                throw new ArgumentException("The user does not contain a list of availabilities");

            if (user.UserAdditionalWorks == null)
                throw new ArgumentException("The user does not contain a list of additional working hours");
        }
    }
}