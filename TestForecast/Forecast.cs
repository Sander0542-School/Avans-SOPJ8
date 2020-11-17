using System;
using System.Collections.Generic;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;

namespace TestForecast
{
    public class Forecast
    {
        private readonly double _coliPerHour;
        private readonly double _customersPerHourCashRegister;
        private readonly double _customersPerHourFresh;
        private readonly double _numberOfCustomersExpected;
        private readonly double _secondsPerMeter;

        private readonly int _expectedColi;
        private readonly List<BranchForecastStandard> _forecastStandards;

        public Forecast(int expectedColi, List<BranchForecastStandard> forecastStandards)
        {
            _expectedColi = expectedColi;
            _forecastStandards = forecastStandards;
            _expectedColi = expectedColi;
            _coliPerHour = 2;
            _customersPerHourCashRegister = 30;
            _customersPerHourFresh = 100;
            _numberOfCustomersExpected = NumberOfCustomersExpected();
            _secondsPerMeter = 0;
        }


        /// <summary>
        /// Calculates the amount of working hours for the cash registers for a specific date
        /// </summary>
        /// <returns> returns a 2 decimal number representing the working hours for the cash registers for a specific date</returns>
        public double WorkHoursCashRegister() => Math.Round(_numberOfCustomersExpected / _customersPerHourCashRegister, 2);

        /// <summary>
        /// Calculates the amount of working hours for the fresh for a specific date
        /// </summary>
        /// <returns> returns a 2 decimal number representing the working hours for the fresh for a specific date</returns>
        public double WorkHoursFresh() => Math.Round(_numberOfCustomersExpected / _customersPerHourFresh,2);

        public int NumberOfCustomersExpected()
        {
            // begin met een standaarthoeveelheid
            // Haal mensen eraf of voeg mensen toe op basis van de dag van de week
            // haal mensen eraf of voeg mensen toe op basis van het weer
            // haal mensen eraf of voeg mensen toe op basis van feestdagen
            return 100;
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
