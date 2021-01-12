using System;
using System.Collections.Generic;
using Bumbo.Web.Models.Schedule;

namespace Bumbo.Web.Models.Home
{
    public class IndexViewModel
    {
        public Dictionary<BranchModel, BranchDataModel> Branches { get; set; }

        public IEnumerable<BirthdayModel> Birthdays { get; set; }

        public IEnumerable<SickModel> Sicks { get; set; }

        public readonly DayOfWeek[] DaysOfWeek =
        {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday,
            DayOfWeek.Saturday,
            DayOfWeek.Sunday
        };
    }

    public class BranchModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
    }

    public class BranchDataModel
    {
        public Dictionary<DayOfWeek, ForecastModel> Forecasts { get; set; }

        public WeatherModel Weather { get; set; }
    }

    public class ForecastModel
    {
        public string ExpectedCrowd
        {
            get
            {
                const int sections = 3;
                var diff = MaxHours - MinHours;

                var sectionDiff = diff / sections;

                if (PlannedHours > (MaxHours - sectionDiff))
                {
                    return "Crowded";
                }
                
                if (PlannedHours > (MaxHours - sectionDiff - sectionDiff))
                {
                    return "Medium";
                }

                return "Quiet";
            }
        }

        public int PlannedHours { get; set; }

        public int MinHours { get; set; }

        public int MaxHours { get; set; }
    }

    public class WeatherModel
    {
        public int Temperature { get; set; }

        public string Icon { get; set; }

        public string IconUrl => $"https://openweathermap.org/img/wn/{Icon}@2x.png";

        public string IconDesc { get; set; }
    }

    public class BirthdayModel
    {
        public DateTime Date { get; set; }

        public string Name { get; set; }
    }

    public class SickModel
    {
        public DateTime Date { get; set; }

        public string Name { get; set; }
    }
}
