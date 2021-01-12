using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Bumbo.Data.Models;
using Bumbo.Logic.Options;
using Bumbo.Logic.Services.Weather.Models;
using Microsoft.Extensions.Options;

namespace Bumbo.Logic.Services.Weather
{
    public class OpenWeatherMapService : IOpenWeatherMapService
    {
        private readonly OpenWeatherMapOptions _options;

        public OpenWeatherMapService(IOptions<OpenWeatherMapOptions> options)
        {
            _options = options.Value;
        }

        public async Task<WeatherResponse> GetWeather(Branch branch)
        {
            try
            {
                var zip = branch.ZipCode.Substring(0, 4);

                using var httpClient = new HttpClient();

                var json = await httpClient.GetStringAsync($"https://api.openweathermap.org/data/2.5/weather?zip={zip},nl&appid={_options.ApiKey}&units=metric");

                return JsonSerializer.Deserialize<WeatherResponse>(json);
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }
    }
}
