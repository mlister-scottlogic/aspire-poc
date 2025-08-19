using Aspire.Hosting;
using AspireApp.Tests.EndToEnd.Database;
using Npgsql;

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

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            var connectionString = await App.GetConnectionStringAsync("postgresdb");

            var dataSource = NpgsqlDataSource.Create(connectionString!);

            var repo = new DailyEntryRepository(dataSource);

            return repo;
        }
    }
}
