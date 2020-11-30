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
        public Branch Branch;
        public int Year;
        public int WeekNr;
        private Dictionary<User, WorkedShift> MonthlyWorkedShiftsPerUser;

        public PaycheckViewModel()
        {
            MonthlyWorkedShiftsPerUser = new Dictionary<User, WorkedShift>();
        }
    }
}
