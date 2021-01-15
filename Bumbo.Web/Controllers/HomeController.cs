using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bumbo.Data;
using Bumbo.Data.Models;
using Bumbo.Logic.Services.Weather;
using Bumbo.Logic.Utils;
using Bumbo.Web.Models;
using Bumbo.Web.Models.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
namespace Bumbo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOpenWeatherMapService _weatherService;
        private readonly RepositoryWrapper _wrapper;

        public HomeController(RepositoryWrapper wrapper, IOpenWeatherMapService weatherService)
        {
            _wrapper = wrapper;
            _weatherService = weatherService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var branches = UserUtil.GetBranches(User).ToList();

            var upcomingBirthdays = await _wrapper.User.GetUpcomingBirthdays(branches, 5);

            var sicks = new Dictionary<DateTime, IEnumerable<User>>
            {
                {
                    DateTime.Today, await _wrapper.User.GetSickEmployees(branches, DateTime.Today)
                },
                {
                    DateTime.Today.AddDays(1), await _wrapper.User.GetSickEmployees(branches, DateTime.Today.AddDays(1))
                }
            };

            var model = new IndexViewModel
            {
                Birthdays = upcomingBirthdays.Select(user => new BirthdayModel
                {
                    Name = UserUtil.GetFullName(user), Date = user.Birthday
                }),
                Sicks = sicks.SelectMany(pair => pair.Value.Select(user => new SickModel
                {
                    Name = UserUtil.GetFullName(user), Date = pair.Key
                })),
                Branches = new Dictionary<BranchModel, BranchDataModel>()
            };

            foreach (var branchId in branches)
            {
                var branch = await _wrapper.Branch.Get(branch1 => branch1.Id == branchId);

                var weather = await _weatherService.GetWeather(await _wrapper.Branch.Get(branch1 => branch1.Id == branch.Id));

                var nextWeek = DateTime.Today.AddDays(7);
                var weekNumber = ISOWeek.GetWeekOfYear(nextWeek);
                var nextMonday = ISOWeek.ToDateTime(nextWeek.Year, weekNumber, DayOfWeek.Monday);
                var nextNextMonday = nextMonday.AddDays(7);

                var forecasts = (await _wrapper.Forecast.GetAll(
                forecast => forecast.BranchId == branch.Id,
                forecast => forecast.Date >= nextMonday,
                forecast => forecast.Date < nextNextMonday)).GroupBy(forecast => forecast.Date).ToList();

                var minHours = forecasts.Any() ? (int)forecasts.Min(grouping => grouping.Sum(forecast => forecast.WorkingHours)) : 0;
                var maxHours = forecasts.Any() ? (int)forecasts.Max(grouping => grouping.Sum(forecast => forecast.WorkingHours)) : 0;

                var branchModel = new BranchModel
                {
                    Id = branch.Id, Name = branch.Name
                };

                var branchDataModel = new BranchDataModel
                {
                    Weather = new WeatherModel
                    {
                        Temperature = (int)(weather?.Main.Temp ?? 0),
                        Icon = weather?.Weather.FirstOrDefault()?.Icon ?? "01d",
                        IconDesc = weather?.Weather.FirstOrDefault()?.Description ?? "clear sky"
                    },
                    Forecasts = forecasts.ToDictionary(grouping => grouping.Key.DayOfWeek, grouping => new ForecastModel
                    {
                        PlannedHours = (int)grouping.Sum(forecast => forecast.WorkingHours),
                        MaxHours = maxHours,
                        MinHours = minHours
                    })
                };

                model.Branches.Add(branchModel, branchDataModel);
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        [HttpPost]
        public IActionResult CultureManagement(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(999)
            });

            return LocalRedirect(returnUrl);
        }
    }
}
