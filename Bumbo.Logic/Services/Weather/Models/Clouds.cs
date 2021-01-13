using System.Text.Json.Serialization;
namespace Bumbo.Logic.Services.Weather.Models
{
    public class Clouds
    {
        [JsonPropertyName("all")]
        public int All { get; set; }
    }
}
