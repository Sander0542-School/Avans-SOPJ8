using System.Text.Json.Serialization;
namespace Bumbo.Logic.Services.Weather.Models
{
    public class Wind
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }

        [JsonPropertyName("deg")]
        public int Deg { get; set; }
    }
}
