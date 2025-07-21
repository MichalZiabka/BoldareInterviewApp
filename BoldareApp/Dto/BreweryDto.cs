using System.Text.Json.Serialization;

namespace BoldareApp.Dto
{
    public record BreweryDto
    {
        [JsonPropertyName("Id")]
        public required string Id { get; init; }

        [JsonPropertyName("Name")]
        public string? Name { get; init; }

        [JsonPropertyName("City")]
        public string? City { get; init; }

        [JsonPropertyName("Phone")]
        public string? PhoneNumber { get; init; }
    }
}
