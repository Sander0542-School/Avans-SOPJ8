using System;
using System.Collections.Generic;
using System.Linq;

namespace Bumbo.Logic.PayCheck
{
    public class PayCheck
    {
        private Dictionary<double, TimeSpan> _workedHoursPerBonus;

        public const double Standard = 1.0;
        public const double Between20And21Bonus = 1.33;
        public const double NightBonus = 1.5; 
        public const double SundayBonus = 2.0;
        public const double SickBonus = 0.70;

        public PayCheck()
        {
            _workedHoursPerBonus = new Dictionary<double, TimeSpan>
            {
                {Standard, new TimeSpan()},
                {Between20And21Bonus, new TimeSpan()},
                {NightBonus, new TimeSpan()},
                {SundayBonus, new TimeSpan()},
                {SickBonus, new TimeSpan()}
            };
        }

        public void AddTime(double key, TimeSpan timeSpan)
        {
            if (!_workedHoursPerBonus.ContainsKey(key))
                throw new ArgumentNullException("KeyNotFound", "The Given Key Does Not exist. Did you try to look for a bonus that is not used?");

            _workedHoursPerBonus[key].Add(timeSpan);
        }

        public TimeSpan GetTime(double key)
        {
            if (!_workedHoursPerBonus.ContainsKey(key))
                throw new ArgumentNullException("KeyNotFound", "The Given Key Does Not exist. Did you try to look for a bonus that is not used?");
            _workedHoursPerBonus.TryGetValue(key, out var hoursAlreadyInTheBonus);
            return hoursAlreadyInTheBonus;
        }

        public void AddPayCheck(PayCheck payCheck)
        {
            foreach (var kvp in payCheck.GetPayCheck())
            {
                AddTime(kvp.Key,kvp.Value);
            }
        }

        public Dictionary<double, TimeSpan> GetPayCheck()
        {
            return _workedHoursPerBonus;
        }

        public List<double> GetKeys()
        {
            return _workedHoursPerBonus.Keys.ToList();
        }
    }
}
