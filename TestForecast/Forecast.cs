using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbo.Logic
{
    public class Forecast
    {
        private readonly double _expectedColi;
        private readonly double _coliPerHour;
        private readonly double _customersPerHourCashRegister;
        private readonly double _customersPerHourFresh;
        private readonly double _numberOfCustomersExpected;
        private readonly double _secondsPerMeter;

        public Forecast(double expectedColi)
        {
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

        public int WorkHoursStockClerk()
        {
            throw new System.NotImplementedException();
        }

        public int NumberOfCustomersExpected()
        {
            // begin met een standaarthoeveelheid
            // Haal mensen eraf of voeg mensen toe op basis van de dag van de week
            // haal mensen eraf of voeg mensen toe op basis van het weer
            // haal mensen eraf of voeg mensen toe op basis van feestdagen
            return 100;
        }
    }
}
