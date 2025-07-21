using System.Text.Json.Serialization;

namespace BoldareApp.Models
{
    public class BreweryModel
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("phone")]
        public string? PhoneNumber { get; set; }
    }
}
