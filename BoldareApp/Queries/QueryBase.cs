namespace BoldareApp.Queries
{
    public abstract class QueryBase
    {
        protected abstract string Path { get; }

        public virtual string BuildQuery()
        {
            var parameters = new List<string>();
            AddParameters(parameters);
            return $"{Path}{string.Join("&", parameters)}";
        }

        protected abstract void AddParameters(List<string> parameters);
    }
}
