using System.Collections.Generic;
using Bumbo.Data.Models;

namespace Bumbo.Web.Models.Forecast
{
    public class ForecastViewModel
    {
        public Branch Branch;
        public int Year;
        public int WeekNr;
        public IEnumerable<Data.Models.Forecast> Forecasts;
    }
}