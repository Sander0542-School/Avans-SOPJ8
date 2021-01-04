using System;
using System.Linq;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;

namespace Bumbo.Logic.ClockSystem
{
    public class ClockSystemLogic
    {
        private readonly RepositoryWrapper _wrapper;

        public ClockSystemLogic(RepositoryWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        public async Task HandleTagScan(User user)
        {
            DateTime scannedDateTime = GetRoundedTimeFromDateTimeNow();

            var shift = user.Shifts.FirstOrDefault(shift1 => shift1.Date == scannedDateTime.Date);

            if (shift != null)
            {
                if (shift.WorkedShift == null)
                {
                    var workedShift = new WorkedShift
                    {
                        ShiftId = shift.Id,
                        StartTime = scannedDateTime.TimeOfDay,
                        Sick = false,
                        IsApprovedForPaycheck = false,
                    };

                    if (await _wrapper.WorkedShift.Add(workedShift) != null)
                    {
                        return;
                    }
                }
                else if (shift.WorkedShift.EndTime == null)
                {
                    shift.WorkedShift.EndTime = scannedDateTime.TimeOfDay;

                    if (await _wrapper.WorkedShift.Update(shift.WorkedShift) != null)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            // if there is no shift then we throw an error.
            throw new ArgumentNullException("the person that is clocking in has no shift for today. try scheduling him first.");
        }

        public DateTime GetRoundedTimeFromDateTimeNow()
        {
            var rawTime = DateTime.Now;

            var remainder = rawTime.Minute % 15;

            var time = remainder < 10 ? rawTime.Subtract(new TimeSpan(0, remainder, 0)) : rawTime.Add(new TimeSpan(0, 15 - remainder, 0));

            return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0);
        }
    }
}
