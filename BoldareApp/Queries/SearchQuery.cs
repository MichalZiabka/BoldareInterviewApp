namespace BoldareApp.Queries
{
    public class SearchQuery : PaginatedQueryBase
    {
        public required string Search { get; init; }

        protected override string Path => "/search?";

        protected override void AddParameters(List<string> parameters)
        {
            parameters.Add($"query={Search}");
        }
    }
}
