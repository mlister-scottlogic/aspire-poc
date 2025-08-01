using AspireApp.ApiService.Data.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace AspireApp.ApiService.Data.Core
{
    internal class EntryContext(DbContextOptions<EntryContext> options) : DbContext(options)
    {
        public DbSet<DailyEntryEntity> DailyEntries { get; set; }
    }
}
