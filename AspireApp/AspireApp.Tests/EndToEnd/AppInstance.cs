using Aspire.Hosting;
using AspireApp.Tests.EndToEnd.Database;
using Microsoft.EntityFrameworkCore;

namespace AspireApp.Tests.EndToEnd
{
    public static class AppInstance
    {
        public static DistributedApplication? App { get; set; }

        public static async Task<DailyEntryRepository> GetDailyEntryRepositoryAsync()
        {
            if (App is null)
            {
                throw new InvalidOperationException(
                    "Must wait for App to be initialised before trying to interact with database"
                );
            }

            var connectionString = await App.GetConnectionStringAsync("postgresdb");

            var options = new DbContextOptionsBuilder<EntryContext>();
            options.UseNpgsql(connectionString);

            var context = new EntryContext(options.Options);

            var repo = new DailyEntryRepository(context);

            return repo;
        }
    }
}
