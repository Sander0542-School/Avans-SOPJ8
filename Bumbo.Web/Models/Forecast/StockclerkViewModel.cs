using System;
using System.Collections.Generic;
namespace Bumbo.Web.Models.Forecast
{
    public class StockclerkViewModel
    {
        public int BranchId;
        public int DaysInForecast;
        public DateTime FirstDayOfWeek;

        // This is not ideal, but binding data to a list of objects in .NET Core seems to be pretty finicky.
        // As this works in our current implementation it should be kept like this for now
        // See this link for previous BROKEN implementation: https://github.com/Sander0542/Bumbo/pull/42/commits/a2f330a2af75f3eb88934aa7dac4f8a494ba2a31
        public List<decimal> MetersOfShelves { get; set; }
        public List<decimal> ExpectedNumberOfColi { get; set; }
        public List<int> ExpectedVisitorPerDay { get; set; }
    }
}
