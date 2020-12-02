using System;
using System.Collections.Generic;
using System.Globalization;
using Bumbo.Data.Models;

namespace Bumbo.Web.Models.Paycheck
{
    public class PaycheckViewModel
    {
        public List<int> WeekNumbers;
        public Branch Branch;
        public User SelectedUser;
        public int Year;
        public int MonthNr;
        public bool OverviewApproved;
        public Dictionary<User, List<WorkedShift>> MonthlyWorkedShiftsPerUser;
        public Dictionary<User, List<double>> WeeklyWorkedHoursPerUser;
        public List<WorkedShift> SelectedUserWorkedShifts;
        public Dictionary<string, string> ResetRouteValues;

        public PaycheckViewModel()
        {
            MonthlyWorkedShiftsPerUser = new Dictionary<User, List<WorkedShift>>();
            SelectedUser = null;
            WeekNumbers = new List<int>();
            WeeklyWorkedHoursPerUser = new Dictionary<User, List<double>>();
            SelectedUserWorkedShifts = new List<WorkedShift>();

            //TODO: Might use?
            ResetRouteValues = new Dictionary<string, string>
            {
                {
                    "monthNr",
                    DateTime.Now.Month.ToString()
                },
                {
                    "year",
                    DateTime.Now.Year.ToString()
                }
            };
        }

        public void GenerateWeeklyWorkedHoursPerUser()
        {
            foreach (var kvp in MonthlyWorkedShiftsPerUser)
            {
                int weekNr = ISOWeek.GetWeekOfYear(kvp.Value[0].Shift.Date);
                int indexWeekNr = 0;

                List<double> workHours = new List<double>();

                for (int i = 0; i < WeekNumbers.Count; i++)
                {
                    workHours.Add(0);
                }

                for (int i = 0; i < kvp.Value.Count; i++)
                {
                    if (weekNr != ISOWeek.GetWeekOfYear(kvp.Value[i].Shift.Date))
                    {
                        weekNr = ISOWeek.GetWeekOfYear(kvp.Value[i].Shift.Date);
                        indexWeekNr++;
                    }

                    var timeDif = kvp.Value[i].EndTime - kvp.Value[i].StartTime;

                    workHours[indexWeekNr] += timeDif.Value.Hours + timeDif.Value.Minutes * 0.01;
                }

                WeeklyWorkedHoursPerUser.Add(kvp.Key, workHours);
            }
        }

        public double CalculateTotalWorkHours(User user)
        {
            double result = 0;

            WeeklyWorkedHoursPerUser.TryGetValue(user, out var weeklyWorkedHours);


            foreach (var hours in weeklyWorkedHours)
            {
                result += hours;
            }

            return result;
        }

        public void SortSelectedUserWorkedShiftsByDate()
        {
            bool changed = false;

            for (int j = 0; j <= SelectedUserWorkedShifts.Count - 2; j++)
            {
                for (int i = 0; i <= SelectedUserWorkedShifts.Count - 2; i++)
                {
                    if (SelectedUserWorkedShifts[i].Shift.Date > SelectedUserWorkedShifts[i + 1].Shift.Date)
                    {
                        var temp = SelectedUserWorkedShifts[i + 1];
                        SelectedUserWorkedShifts[i + 1] = SelectedUserWorkedShifts[i];
                        SelectedUserWorkedShifts[i] = temp;
                        changed = true;
                    }
                }

                if (!changed)
                {
                    return;
                }
            }
        }
    }
}
