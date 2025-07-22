using System.ComponentModel.DataAnnotations;

namespace BoldareApp.Queries
{
    public class FilterQuery : PaginatedQueryBase
    {
        [MinLength(3, ErrorMessage = "Parameter must be at least 3 characters long")]
        public string? Name { get; init; }

        [MinLength(3, ErrorMessage = "Parameter must be at least 3 characters long")]
        public string? City { get; init; }

        public string? Sort { get; init; } = "name";

        public string? Order { get; init; } = "asc";

        protected override string Path => "?";

        protected override void AddParameters(List<string> parameters)
        {
            if (!string.IsNullOrEmpty(Name)) parameters.Add($"by_name={Name}");
            if (!string.IsNullOrEmpty(City)) parameters.Add($"by_city={City}");
            parameters.Add($"sort={Sort}:{Order}");
        }
    }
}
