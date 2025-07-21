namespace BoldareApp.Queries
{
    public abstract class PaginatedQueryBase : QueryBase
    {
        public int Page { get; init; } = 1;

        public int PageSize { get; init; } = 10;

        public override string BuildQuery()
        {
            var parameters = new List<string>();

            AddParameters(parameters);

            parameters.Add($"page={Page}");
            parameters.Add($"per_page={PageSize}");

            return $"{Path}{string.Join("&", parameters)}";
        }
    }
}
