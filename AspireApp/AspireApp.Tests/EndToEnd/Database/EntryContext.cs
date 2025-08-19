using Microsoft.EntityFrameworkCore;

namespace AspireApp.Tests.EndToEnd.Database
{
    public class EntryContext(DbContextOptions<EntryContext> options) : DbContext(options)
    {
        public DbSet<DailyEntryEntity> DailyEntries { get; set; }
    }
}
