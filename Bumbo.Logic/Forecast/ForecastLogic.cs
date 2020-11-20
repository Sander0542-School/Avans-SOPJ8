using System;
using System.Collections.Generic;
using System.Linq;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Logic.Forecast
{
    public class ForecastLogic
    {
        private readonly decimal _minutesPerColiUnloading;
        private readonly decimal _minutesPerColiStockShelves;
        private readonly decimal _customersPerHourCashRegister;
        private readonly decimal _customersPerHourProduceDepartment;
        private readonly decimal _secondsPerMeterFacing;

        /// <summary>
        /// All the holiday dates
        /// </summary>
        private static IEnumerable<DateTime> holidays = new List<DateTime>
        {
            new DateTime(2021, 01, 01), // New Year's Day
            new DateTime(2021, 01, 06), // Three Kings
            new DateTime(2021, 02, 14), // Valentine
            new DateTime(2021, 02, 23), // Carnival
            new DateTime(2021, 03, 10), // Good Friday
            new DateTime(2021, 04, 12), // Easter 1st Day
            new DateTime(2021, 04, 13), // Easter 2nd Day
            new DateTime(2021, 04, 24), // Ramadan
            new DateTime(2021, 04, 27), // King Day
            new DateTime(2021, 05, 01), // Day Of Labor
            new DateTime(2021, 05, 04), // Remembrance Day
            new DateTime(2021, 05, 05), // Liberty Day
            new DateTime(2021, 05, 10), // Mother Day
            new DateTime(2021, 05, 21), // Ascension Day
            new DateTime(2021, 05, 23), // Sugar Party 
            new DateTime(2021, 05, 31), // Pink Stars 1st Day
            new DateTime(2021, 06, 01), // Pink Stars 2st Day
            new DateTime(2021, 06, 21), // Daddy Day
            new DateTime(2021, 06, 30), // Sacrifice feast
            new DateTime(2021, 09, 15), // Prince Day
            new DateTime(2021, 10, 04), // Day Of The Animal
            new DateTime(2021, 10, 31), // Halloween
            new DateTime(2021, 11, 11), // Sint Martin
            new DateTime(2021, 12, 05), // Sint Klaas
            new DateTime(2021, 12, 25), // Christmas 1st Day
            new DateTime(2021, 12, 26), // Christmas 2st Day
            new DateTime(2020, 12, 31), // New Year's Day
        };

        /// <summary>
        /// The number of decimals to which returned values will be rounded.
        /// </summary>
        //private static readonly int RoundingFactor = 2;

        public ForecastLogic(List<BranchForecastStandard> forecastStandards)
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
                        throw new ArgumentOutOfRangeException(nameof(forecastStandards), "ForecastLogic encountered an unknown enum value. Did you add a new enum to ForecastActivity?");
                }
            }
        }

        public decimal GetWorkHoursCashRegister(DateTime date)
        {
            return WorkHoursCashRegister(NumberOfCustomersExpected(date));
        }

        public decimal GetWorkHoursFresh(DateTime date)
        {
            return WorkHoursFresh(NumberOfCustomersExpected(date));
        }
        public decimal GetWorkHoursStockClerk(decimal metersOfShelves, decimal expectedNumberOfColi)
        {
            return WorkHoursUnloading(expectedNumberOfColi) + WorkHoursFacingShelves(metersOfShelves) + WorkHoursStockingColi(expectedNumberOfColi);
        }


        /// <summary>
        /// Calculates the number of hours which is required for the amount of coli that is expected.
        /// </summary>
        /// <param name="expectedNumberOfColi">Number of coli that is expected</param>
        /// <returns>Number of hours required to unload all the coli</returns>
        public decimal WorkHoursUnloading(decimal expectedNumberOfColi) => expectedNumberOfColi * _minutesPerColiUnloading / 60;

        /// <summary>
        /// Calculates the amount of working hours for the cash registers for a specific date
        /// </summary>
        /// <param name="numberOfCustomersExpected">Number of expected customers for a given day</param>
        /// <returns> returns a 2 decimal number representing the working hours for the cash registers for a specific date</returns>
        public decimal WorkHoursCashRegister(decimal numberOfCustomersExpected) => numberOfCustomersExpected / _customersPerHourCashRegister;

        /// <summary>
        /// Calculates the amount of working hours for the fresh for a specific date
        /// </summary>
        /// <param name="numberOfCustomersExpected">Number of expected customers for a given day</param>
        /// <returns> returns a 2 decimal number representing the working hours for the fresh for a specific date</returns>
        public decimal WorkHoursFresh(decimal numberOfCustomersExpected) => numberOfCustomersExpected / _customersPerHourProduceDepartment;


        public int NumberOfCustomersExpected(DateTime date)
        {
            // begin met een standaarthoeveelheid
            int _numberOfCustomersExpected = 1785;

            // Haal mensen eraf of voeg mensen toe op basis van de dag van de week
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    _numberOfCustomersExpected = (int)(_numberOfCustomersExpected * 0.90);
                    break;
                case DayOfWeek.Tuesday:
                    _numberOfCustomersExpected = (int)(_numberOfCustomersExpected * 0.95);
                    break;
                case DayOfWeek.Wednesday:
                    _numberOfCustomersExpected = (int)(_numberOfCustomersExpected * 1.0);
                    break;
                case DayOfWeek.Thursday:
                    _numberOfCustomersExpected = (int)(_numberOfCustomersExpected * 1.05);
                    break;
                case DayOfWeek.Friday:
                    _numberOfCustomersExpected = (int)(_numberOfCustomersExpected * 1.10);
                    break;
                case DayOfWeek.Saturday:
                    _numberOfCustomersExpected = (int)(_numberOfCustomersExpected * 1.0);
                    break;
                default:
                    _numberOfCustomersExpected = (int)(_numberOfCustomersExpected * 0.90);
                    break;
            }

            // TODO: haal mensen eraf of voeg mensen toe op basis van het weer

            // haal mensen eraf of voeg mensen toe op basis van feestdagen
            if (holidays.FirstOrDefault(predicate: e => e.Date.Subtract(new TimeSpan(1,0,0,0)) == date) != null)
            {
                _numberOfCustomersExpected = (int)(_numberOfCustomersExpected * 1.20);
            }

            return _numberOfCustomersExpected;
        }

        /// <summary>
        /// Calculates the total number of hours that need to be worked depending on the amount of coli that's expected
        /// </summary>
        /// <param name="expectedNumberOfColi">Number of coli which needs to be stocked in shelves</param>
        /// <returns>Total number of hours required to work to stock all the coli. Specific to two decimals</returns>
        public decimal WorkHoursStockingColi(decimal expectedNumberOfColi) => expectedNumberOfColi * _minutesPerColiStockShelves / 60;

        /// <summary>
        /// Calculates the number of hours which is spent facing a given distance of shelves
        /// </summary>
        /// <param name="metersOfShelves">The amount of shelves in meters which need to be faced</param>
        /// <returns>Number of hours required to face shelves</returns>
        public decimal WorkHoursFacingShelves(decimal metersOfShelves) => _secondsPerMeterFacing * metersOfShelves / 60;
    }
}
