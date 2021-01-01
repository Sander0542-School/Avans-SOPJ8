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

        // method die een user krijgt en op basis hiervan kijkt of er al een worked shift bestaat voor die datum
        // zo ja zet de eind tijd en zo nee maak een nieuwe worked shift aan
        public async Task HandleTagScan(User user)
        {
            DateTime scannedDateTime = GetRoundedTimeFromDateTimeNow();

            var shift = user.Shifts.Where(myShift => myShift.Date == scannedDateTime.Date).ToList().FirstOrDefault();

            if (shift != null)
            {
                var workedShift = await _wrapper.WorkedShift.Get(myWorkedShift => myWorkedShift.ShiftId == shift.Id);

                if (workedShift.EndTime == null)
                {
                    workedShift.EndTime = scannedDateTime.TimeOfDay;
                    await _wrapper.WorkedShift.Update(workedShift);
                    return;
                }

                await _wrapper.WorkedShift.Add(new WorkedShift
                {
                    StartTime = scannedDateTime.TimeOfDay,
                    ShiftId = shift.Id,
                    Shift = shift,
                    Sick = false,
                    IsApprovedForPaycheck = false,
                });
                return;
            }

            // if there is no shift then we throw an error.
            throw new ArgumentNullException("the person that is clocking in has no shift for today. try scheduling him first.");
        }

        public DateTime GetRoundedTimeFromDateTimeNow()
        {
            DateTime rawTime = DateTime.Now;

            var remainder = rawTime.Minute % 15;

            return remainder < 10 ? rawTime.Subtract(new TimeSpan(0, remainder, 0)) : rawTime.Add(new TimeSpan(0, 15 - remainder, 0));
        }
    }
}
