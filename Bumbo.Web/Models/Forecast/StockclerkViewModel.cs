using System;
using System.Collections.Generic;

namespace Bumbo.Web.Models.Forecast
{
    public class StockclerkViewModel
    {
        public DateTime FirstDayOfWeek;
        public int BranchId;
        /// <summary>
        /// Number of days you want to generate a forecast for
        /// </summary>
        public int DaysInForecast = 7;

        public List<ForecastInput> ForecastInputs;

        public StockclerkViewModel()
        {
            ForecastInputs = new List<ForecastInput>();

            for (var i = 0; i < DaysInForecast; i++)
            {
                ForecastInputs.Add(new ForecastInput());
            }
        }
        
        public struct ForecastInput
        {
            public decimal MetersOfShelves;
            public decimal ExpectedNumberOfColi;
        } 
    }
}
