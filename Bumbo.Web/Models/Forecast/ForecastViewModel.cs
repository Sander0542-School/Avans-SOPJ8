using System;
using System.Collections.Generic;
using Bumbo.Data.Models;
using Bumbo.Logic.Forecast;
using Microsoft.VisualBasic;

namespace Bumbo.Web.Models.Forecast
{
    public class ForecastViewModel
    {
        public Branch Branch;
        public int Year;
        public int WeekNr;
        public IEnumerable<Data.Models.Forecast> Forecasts;
        public Dictionary<string, string> resetRouteValues;

        public ForecastViewModel()
        {
            resetRouteValues = new Dictionary<string, string>()
            {
                {
                    "weekNr",
                    DateLogic.GetWeekNumber(DateTime.Now).ToString()
                },
                {
                    "year",
                    DateTime.Now.Year.ToString()
                }
            };
        }
    }
}