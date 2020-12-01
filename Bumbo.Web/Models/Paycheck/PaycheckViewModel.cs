using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Web.Models.Paycheck
{
    public class PaycheckViewModel
    {
        public List<int> WeekNumbers;
        public Branch Branch;
        public int Year;
        public int WeekNr;
        public Dictionary<User, List<WorkedShift>> MonthlyWorkedShiftsPerUser;
        public Dictionary<User, List<int>> WeeklyWorkedHoursPerUser;
        public User SelectedUser;

        public PaycheckViewModel()
        {
            MonthlyWorkedShiftsPerUser = new Dictionary<User, List<WorkedShift>>();
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
