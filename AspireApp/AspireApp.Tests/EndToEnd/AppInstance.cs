using Aspire.Hosting;
using AspireApp.Constants;
using AspireApp.Tests.EndToEnd.Database;
using AspireApp.Tests.EndToEnd.Messaging;
using Npgsql;
using RabbitMQ.Client;

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

            var connectionString = await App.GetConnectionStringAsync(AspireConstants.Database);

            var dataSource = NpgsqlDataSource.Create(connectionString!);

            var repo = new DailyEntryRepository(dataSource);

            return repo;
        }

        private static TestQueueListener? _testQueueListener;

        public static async Task<TestQueueListener> GetTestQueueListenerAsync()
        {
            if (_testQueueListener != null)
            {
                return _testQueueListener;
            }

            if (App is null)
            {
                throw new InvalidOperationException(
                    "Must wait for App to be initialised before trying to interact with database"
                );
            }

            var connectionString = await App.GetConnectionStringAsync(AspireConstants.Messaging);

            if (connectionString is null)
            {
                throw new InvalidOperationException("No RabbitMQ connection string");
            }

            var factory = new ConnectionFactory() { Uri = new(connectionString) };

            var connection = await factory.CreateConnectionAsync();
            var testQueueListener = new TestQueueListener(connection);

            _testQueueListener = testQueueListener;
            return testQueueListener;
        }
    }
}
