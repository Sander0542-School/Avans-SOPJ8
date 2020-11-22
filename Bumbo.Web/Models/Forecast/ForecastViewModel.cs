using System;
using System.Collections.Generic;
using System.Globalization;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Enums;

namespace Bumbo.Web.Models.Forecast
{
    public class ForecastViewModel
    {
        public Branch Branch;
        public Department? Department;
        public int Year;
        public int WeekNr;
        public IEnumerable<Data.Models.Forecast> Forecasts;
        public Dictionary<string, string> ResetRouteValues;

        public ForecastViewModel()
        {
            ResetRouteValues = new Dictionary<string, string>
            {
                {
                    "weekNr",
                    ISOWeek.GetWeekOfYear(DateTime.Now).ToString()
                },
                {
                    "year",
                    DateTime.Now.Year.ToString()
                }
            };
        }
    }
}