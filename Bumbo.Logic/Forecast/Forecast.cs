using System;
using System.Collections.Generic;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;

namespace TestForecast
{
    public class Forecast
    {
        private readonly double _minutesPerColiUnloading;
        private readonly double _minutesPerColiStockShelves;
        private readonly double _customersPerHourCashRegister;
        private readonly double _customersPerHourProduceDepartment;
        private readonly double _secondsPerMeterFacing;

        /// <summary>
        /// The number of decimals to which returned values will be rounded.
        /// </summary>
        private static readonly int RoundingFactor = 2;

        public Forecast(List<BranchForecastStandard> forecastStandards)
        {
            foreach (var f in forecastStandards)
            {
                switch (f.Activity)
                {
                    case ForecastActivity.UNLOAD_COLI:
                        _minutesPerColiUnloading = f.Value;
                        break;
                    case ForecastActivity.STOCK_SHELVES:
                        _minutesPerColiStockShelves = f.Value;
                        break;
                    case ForecastActivity.CASHIER:
                        _customersPerHourCashRegister = f.Value;
                        break;
                    case ForecastActivity.PRODUCE_DEPARTMENT:
                        _customersPerHourProduceDepartment = f.Value;
                        break;
                    case ForecastActivity.FACE_SHELVES:
                        _secondsPerMeterFacing = f.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(forecastStandards), "Forecast encountered an unknown enum value. Did you add a new enum to ForecastActivity?");
                }
            }
        }

        /// <summary>
        /// Calculates the number of hours which is required for the amount of coli that is expected.
        /// </summary>
        /// <param name="expectedNumberOfColi">Number of coli that is expected</param>
        /// <returns>Number of hours required to unload all the coli</returns>
        public double WorkHoursUnloading(double expectedNumberOfColi) =>
            Math.Round(expectedNumberOfColi * _minutesPerColiUnloading / 60, RoundingFactor);

        /// <summary>
        /// Calculates the amount of working hours for the cash registers for a specific date
        /// </summary>
        /// <param name="numberOfCustomersExpected">Number of expected customers for a given day</param>
        /// <returns> returns a 2 decimal number representing the working hours for the cash registers for a specific date</returns>
        public double WorkHoursCashRegister(double numberOfCustomersExpected) => Math.Round(numberOfCustomersExpected / _customersPerHourCashRegister, RoundingFactor);

        /// <summary>
        /// Calculates the amount of working hours for the fresh for a specific date
        /// </summary>
        /// <param name="numberOfCustomersExpected">Number of expected customers for a given day</param>
        /// <returns> returns a 2 decimal number representing the working hours for the fresh for a specific date</returns>
        public double WorkHoursFresh(double numberOfCustomersExpected) => Math.Round(numberOfCustomersExpected / _customersPerHourProduceDepartment, RoundingFactor);


        // TODO: Deze methode moet of verijderd worden of een nuttige implementatie krijgen
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
        /// <param name="expectedNumberOfColi">Number of coli which needs to be stocked in shelves</param>
        /// <returns>Total number of hours required to work to stock all the coli. Specific to two decimals</returns>
        public double WorkHoursStockClerk(double expectedNumberOfColi) => Math.Round(expectedNumberOfColi * _minutesPerColiStockShelves / 60, RoundingFactor);

        /// <summary>
        /// Calculates the number of hours which is spent facing a given distance of shelves
        /// </summary>
        /// <param name="metersOfShelves">The amount of shelves in meters which need to be faced</param>
        /// <returns>Number of hours required to face shelves</returns>
        public double WorkHoursFacingShelves(double metersOfShelves) =>
            Math.Round(_secondsPerMeterFacing * metersOfShelves / 60, RoundingFactor);
    }
}
