using System;
using System.Collections.Generic;

namespace Bumbo.Web.Models.Forecast
{
    public class StockclerkViewModel
    {
        public DateTime FirstDayOfWeek;
        public int BranchId;
        public int DaysInForecast;

        public List<decimal> MetersOfShelves { get; set; }
        public List<decimal> ExpectedNumberOfColi { get; set; }
    }
}
