using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbo.Logic.Utils
{
    public class PayCheck
    {
        private Dictionary<double, TimeSpan> _workedHoursPerBonus;
        private List<double> _keys = new List<double>
        {
            1.0,
            1.33,
            1.5,
            2.0,
            0.70
        }; 

        public PayCheck()
        {
            _workedHoursPerBonus = new Dictionary<double, TimeSpan>
            {
                {_keys[0], new TimeSpan()},
                {_keys[1], new TimeSpan()},
                {_keys[2], new TimeSpan()},
                {_keys[3], new TimeSpan()},
                {_keys[4], new TimeSpan()}
            };
        }

        public void AddTime(double key, TimeSpan timeSpan)
        {
            if (_workedHoursPerBonus.ContainsKey(key))
                throw new ArgumentNullException("KeyNotFound", "The Given Key Does Not exist. Did you try to look for a bonus that is not used?");
            _workedHoursPerBonus.TryGetValue(key, out var hoursAlreadyInTheBonus);
            _workedHoursPerBonus.Remove(key);
            _workedHoursPerBonus.Add(key, hoursAlreadyInTheBonus + timeSpan);
        }

        public TimeSpan GetTime(double key)
        {
            if (_workedHoursPerBonus.ContainsKey(key))
                throw new ArgumentNullException("KeyNotFound", "The Given Key Does Not exist. Did you try to look for a bonus that is not used?");
            _workedHoursPerBonus.TryGetValue(key, out var hoursAlreadyInTheBonus);
            return hoursAlreadyInTheBonus;
        }

        public void AddPayCheck(PayCheck payCheck)
        {
            foreach (var kvp in payCheck.getPayCheck())
            {
                AddTime(kvp.Key,kvp.Value);
            }
        }

        public Dictionary<double, TimeSpan> getPayCheck()
        {
            return _workedHoursPerBonus;
        }

        public List<double> getKeys()
        {
            return _keys;
        }
    }
}
