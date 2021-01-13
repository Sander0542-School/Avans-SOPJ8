using System.Threading.Tasks;
using Bumbo.Data.Models;
using Bumbo.Logic.Services.Weather.Models;
namespace Bumbo.Logic.Services.Weather
{
    public interface IOpenWeatherMapService
    {
        Task<WeatherResponse> GetWeather(Branch branch);
    }
}
