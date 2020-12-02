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
        public int MonthNr;
        public Dictionary<User, List<WorkedShift>> MonthlyWorkedShiftsPerUser;
        public Dictionary<User, List<int>> WeeklyWorkedHoursPerUser;
        public User SelectedUser;
        public List<WorkedShift> SelectedUserWorkedShifts;

        public PaycheckViewModel()
        {
            MonthlyWorkedShiftsPerUser = new Dictionary<User, List<WorkedShift>>();
            SelectedUser = null;
            WeekNumbers = new List<int>();
            WeeklyWorkedHoursPerUser = new Dictionary<User, List<int>>();
            SelectedUserWorkedShifts = new List<WorkedShift>();
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

        public void SortSelectedUserWorkedShiftsByDate()
        {

        }
    }
}
