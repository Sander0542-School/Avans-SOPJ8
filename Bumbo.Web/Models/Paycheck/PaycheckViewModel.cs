using System.Collections.Generic;
using Bumbo.Data.Models;

namespace Bumbo.Web.Models.Paycheck
{
    public class PaycheckViewModel
    {
        public List<int> WeekNumbers;
        public Branch Branch;
        public int Year;
        public int MonthNr;
        public Dictionary<User, List<WorkedShift>> MonthlyWorkedShiftsPerUser;
        public Dictionary<User, List<int>> WeeklyWorkedHoursPerUser;
        public User SelectedUser;

        public PaycheckViewModel()
        {
            MonthlyWorkedShiftsPerUser = new Dictionary<User, List<WorkedShift>>();
            WeeklyWorkedHoursPerUser = new Dictionary<User, List<int>>();
            WeekNumbers = new List<int>();
            SelectedUser = null;
        }

        public void GenerateWeeklyWorkedHoursPerUser()
        {

        }

        public int CalculateTotalWorkHours(User user)
        {
            int result = 0;

            WeeklyWorkedHoursPerUser.TryGetValue(user, out var weeklyWorkedHours);


            foreach (var hours in weeklyWorkedHours)
            {
                result += hours;
            }

            return result;
        }
    }
}
