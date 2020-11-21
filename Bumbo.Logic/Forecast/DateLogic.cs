using System;
using System.Globalization;

namespace Bumbo.Logic.Forecast
{
    public class DateLogic
    {
        public static bool DateIsInSameWeek(DateTime date1, DateTime date2)
        {
            if (date1.Year != date2.Year) return false;
            return GetWeekNumber(date1) == GetWeekNumber(date2);
        } 

        // https://stackoverflow.com/q/11154673/10557332
        public static int GetWeekNumber(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        
        public static DateTime DateFromWeekNumber(int year, int weekOfYear)
        {
            if(weekOfYear <= 0) throw new ArgumentException("Week cannot be negative or zero", nameof(weekOfYear));

            var jan1 = new DateTime(year, 1, 1);
            var daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            var firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            var firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            var result = firstThursday.AddDays(weekNum * 7 - 3);

            if (result.Year != year) throw new ArgumentException("Week number lies in next year", nameof(year));

            return result;
        }

    }
}