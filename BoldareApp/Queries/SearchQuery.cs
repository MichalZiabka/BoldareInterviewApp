using System.ComponentModel.DataAnnotations;

namespace BoldareApp.Queries
{
    public class SearchQuery : PaginatedQueryBase
    {
        [MinLength(3, ErrorMessage = "Parameter must be at least 3 characters long")]
        public required string Search { get; init; }

        protected override string Path => "/search?";

        protected override void AddParameters(List<string> parameters)
        {
            parameters.Add($"query={Search}");
        }
    }
}
