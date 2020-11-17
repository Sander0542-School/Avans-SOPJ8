using System;
using System.Collections.Generic;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;

namespace TestForecast
{
    public class Forecast
    {
        private readonly int _expectedColi;
        private readonly List<BranchForecastStandard> _forecastStandards;

        public Forecast(int expectedColi, List<BranchForecastStandard> forecastStandards)
        {
            _expectedColi = expectedColi;
            _forecastStandards = forecastStandards;
        }

        public int WorkHoursCashRegister()
        {
            throw new System.NotImplementedException();
        }

        public int WorkHoursFresh()
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Calculates the total number of hours that need to be worked depending on the amount of coli that's expected
        /// </summary>
        /// <returns>Total number of hours required to work to stock all the coli. Specific to two decimals</returns>
        public double WorkHoursStockClerk()
        {
            var stockForecast = _forecastStandards.Find(f => f.Activity == ForecastActivity.STOCK_SHELVES);
            if(stockForecast == null)
                throw new NullReferenceException("Forecast activity for STOCK_SHELVES was null");
            var minutesPerColi = stockForecast.Value;

            return Math.Round((double)_expectedColi * minutesPerColi / 60, 2);
        }
    }
}
