using Bumbo.Data.Models;
using Bumbo.Logic.EmployeeRules;
using System;

namespace Bumbo.Logic.PayCheck
{
    public class PayCheckLogic
    {
        public static double CalculateBonus(WorkedShift workedShift)
        {
            if (!workedShift.EndTime.HasValue) throw new ArgumentNullException(nameof(workedShift.EndTime), "Bonus can't be calculated for a shift that hasn't ended yet. EndTime cannot bes Null");

            var billableTime = workedShift.EndTime.Value - workedShift.StartTime - BreakDuration.GetDuration(workedShift.Shift);

            // TODO: Finish method

            return 0;
        }

        /// <summary>
        /// Calculates how much time is spent within 00:00 and 06:00 and returns the amount of extra hours 
        /// </summary>
        /// <param name="workedShift">Shift to calculate the extra billable time for</param>
        /// <exception cref="ArgumentNullException">Throws exception if EndTime is not defined.</exception>
        /// <returns>Extra amount of billable time that can be added to the shift</returns>
        private static TimeSpan BonusTimeBetween00And06(WorkedShift workedShift)
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
    }
}
