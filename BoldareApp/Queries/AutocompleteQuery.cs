namespace BoldareApp.Queries
{
    public class AutocompleteQuery : QueryBase
    {
        public required string Search { get; init; }

        protected override string Path => "/autocomplete?";

        protected override void AddParameters(List<string> parameters)
        {
            parameters.Add($"query={Search}");
        }
    }
}
