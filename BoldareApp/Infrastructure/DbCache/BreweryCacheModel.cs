namespace BoldareApp.Infrastructure.DbCache
{
    public class BreweryCacheModel
    {
        public int Id { get; set; }
        public required string CacheKey { get; set; }
        public required string JsonData { get; set; }
        public DateTime CachedAt { get; set; }
    }
}
