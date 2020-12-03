using System;
using Bumbo.Data.Models;

namespace Bumbo.Logic.EmployeeRules
{
    public static class BreakDuration
    {
        public static TimeSpan GetDuration(TimeSpan shiftDuration)
        {
            var minutes = 0;

            if (shiftDuration.TotalMinutes >= 270) // 4.5 hours
            {
                minutes = 30;
            }

            if (shiftDuration.TotalMinutes >= 480) // 8 hours
            {
                minutes = 60;
            }

            return new TimeSpan(0, minutes, 0);
        }

        public static TimeSpan GetDuration(DateTime startTime, DateTime endTime) => GetDuration(endTime - startTime);

        public static TimeSpan GetDuration(TimeSpan startTime, TimeSpan endTime) => GetDuration(endTime - startTime);

        public static TimeSpan GetDuration(Shift shift) => GetDuration(shift.EndTime - shift.StartTime);
    }
}