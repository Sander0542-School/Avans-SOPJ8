using Bumbo.Data.Models;
using System;
using System.Linq.Expressions;

namespace Bumbo.Logic.PayCheck
{
    public class PayCheckLogic
    {
        public PayCheck CalculateBonus(WorkedShift workedShift)
        {
            if (!workedShift.EndTime.HasValue) throw new ArgumentNullException(nameof(workedShift.EndTime), "Bonus can't be calculated for a shift that hasn't ended yet. EndTime cannot be null");

            PayCheck payCheck = new PayCheck();

            //var billableTime = workedShift.EndTime.Value - workedShift.StartTime - BreakDuration.GetDuration(workedShift.Shift);

            if (workedShift.Sick)
            {
                payCheck.AddTime(0.70, workedShift.EndTime.Value - workedShift.StartTime);
            }
            else if (workedShift.Shift.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                payCheck.AddTime(2.0, workedShift.EndTime.Value - workedShift.StartTime);
            }
            else if (workedShift.Shift.Date.DayOfWeek == DayOfWeek.Saturday)
            {
                payCheck.AddTime(1.5, BonusTimeBetween18And24(workedShift));
                payCheck.AddTime(1.5, BonusTimeBetween00And06(workedShift));

                TimeSpan startTime = workedShift.StartTime.Hours > 6
                    ? workedShift.StartTime
                    : new TimeSpan(6, 0, 0);

                TimeSpan endTime = workedShift.EndTime.Value.Hours > 18
                    ? new TimeSpan(18, 0, 0)
                    : workedShift.EndTime.Value;

                var result = endTime - startTime;

                _ = result.TotalMinutes < 0 ? result = new TimeSpan(0, 0, 0) : result;

                payCheck.AddTime(1.0, result);
            }
            else
            {
                payCheck.AddTime(1.5, BonusTimeBetween00And06(workedShift));
                payCheck.AddTime(1.33, BonusTimeBetween20And21(workedShift));
                payCheck.AddTime(1.5, BonusTimeBetween21And24(workedShift));

                TimeSpan startTime = workedShift.StartTime.Hours > 6
                    ? workedShift.StartTime
                    : new TimeSpan(6, 0, 0);

                TimeSpan endTime = workedShift.EndTime.Value.Hours > 18
                    ? new TimeSpan(18, 0, 0)
                    : workedShift.EndTime.Value;

                var result = endTime - startTime;

                _ = result.TotalMinutes < 0 ? result = new TimeSpan(0, 0, 0) : result;

                payCheck.AddTime(1.0, result);
            }

            return payCheck;
        }

        /// <summary>
        /// Calculates how much time is spent within 00:00 and 06:00 and returns the amount of extra hours 
        /// </summary>
        /// <param name="workedShift">Shift to calculate the extra billable time for</param>
        /// <exception cref="ArgumentNullException">Throws exception if EndTime is not defined.</exception>
        /// <returns>Extra amount of billable time that can be added to the shift</returns>
        public TimeSpan BonusTimeBetween00And06(WorkedShift workedShift)
        {
            if (!workedShift.EndTime.HasValue) throw new ArgumentNullException(nameof(workedShift.EndTime));

            // Check if shift spans across multiple days
            // Assumption: Employees never work more than 24 hours in one shift
            var multiDayShift = workedShift.StartTime > workedShift.EndTime.Value;
            var validTimeFrame = multiDayShift;

            // Check if shift started before 6:00
            if (!validTimeFrame) validTimeFrame = workedShift.StartTime.Hours < 6;

            if (!validTimeFrame) return new TimeSpan();

            // Time started working within the time frame that allocates the bonus 
            var startTime = multiDayShift ? new TimeSpan() : workedShift.StartTime;
            
            var endTime = workedShift.EndTime.Value.Hours > 6 ? new TimeSpan(6, 0, 0) : workedShift.EndTime.Value;

            return endTime - startTime;
        }

        public TimeSpan BonusTimeBetween18And24(WorkedShift workedShift)
        {
            if (!workedShift.EndTime.HasValue) throw new ArgumentNullException(nameof(workedShift.EndTime));

            // Time started working within the time frame that allocates the bonus 
            var startTime = workedShift.StartTime.Hours < 18 ? new TimeSpan(18,0,0) : workedShift.StartTime;

            var result = workedShift.EndTime.Value - startTime;
            return result.TotalMinutes < 0 ? new TimeSpan(0,0,0) : result;
        }

        public TimeSpan BonusTimeBetween20And21(WorkedShift workedShift)
        {
            if (!workedShift.EndTime.HasValue) throw new ArgumentNullException(nameof(workedShift.EndTime));

            // Time started working within the time frame that allocates the bonus 
            var startTime = workedShift.StartTime.Hours < 20 ? new TimeSpan(20, 0, 0) : workedShift.StartTime;

            var endTime = workedShift.EndTime.Value.Hours > 20 ? new TimeSpan(21, 0, 0) : workedShift.EndTime.Value;

            var result = endTime - startTime;
            return result.TotalMinutes < 0 ? new TimeSpan(0, 0, 0) : result;
        }

        public TimeSpan BonusTimeBetween21And24(WorkedShift workedShift)
        {
            if (!workedShift.EndTime.HasValue) throw new ArgumentNullException(nameof(workedShift.EndTime));

            // Time started working within the time frame that allocates the bonus 
            var startTime = workedShift.StartTime.Hours < 21 ? new TimeSpan(21, 0, 0) : workedShift.StartTime;

            var result = workedShift.EndTime.Value - startTime;
            return result.TotalMinutes < 0 ? new TimeSpan(0, 0, 0) : result;
        }
    }
}
