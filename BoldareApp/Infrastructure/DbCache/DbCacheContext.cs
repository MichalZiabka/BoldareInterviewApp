using Microsoft.EntityFrameworkCore;

namespace BoldareApp.Infrastructure.DbCache
{
    public class DbCacheContext : DbContext
    {
        public DbCacheContext(DbContextOptions<DbCacheContext> options) : base(options) { }

        public DbSet<BreweryCacheModel> BreweryCache { get; set; }
    }
}
