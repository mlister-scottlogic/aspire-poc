using Aspire.Hosting;
using AspireApp.ApiService.Contracts;
using AspireApp.Tests.Performance.LoadFramework;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace AspireApp.Tests.Performance
{
    public class LoadTest : IDisposable
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(60);

        private DistributedApplication _application;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _application = await StartupAppAsync();
        }

        [Test]
        public async Task EntriesLoadTestAsync()
        {
            var cancellationToken = TestContext.CurrentContext.CancellationToken;

            using var httpClient = _application.CreateHttpClient("apiservice");

            await _application
                .ResourceNotifications.WaitForResourceHealthyAsync("apiservice", cancellationToken)
                .WaitAsync(DefaultTimeout, cancellationToken);

            var dailyEntry = new DailyEntry()
            {
                Title = "Something",
                Description = "Something else",
                Date = DateOnly.Parse("2020-02-01"),
                Distance = 10.2m,
                DistanceUnit = ApiService.Domain.Enums.DistanceUnit.Miles,
            };

            var loadScenario = HttpLoadScenario.Create(
                () => httpClient.PostAsJsonAsync("/entries", dailyEntry),
                concurrentRequests: 10,
                delayBetweenCalls: TimeSpan.FromMilliseconds(5),
                duration: TimeSpan.FromMinutes(1)
            );

            var result = await loadScenario.RunAsync(warmup: true);

            Console.WriteLine(result);

            var millisecondAverageLimit = 100;
            var failureLimit = 3;

            Assert.Multiple(() =>
            {
                Assert.That(
                    result.Failures < failureLimit,
                    $"Expected fewer than {failureLimit} failures, but found {result.Failures}"
                );
                Assert.That(
                    result.Average < millisecondAverageLimit,
                    $"Expected less than {millisecondAverageLimit} average, but found {result.Average}"
                );
            });
        }

        private static async Task<DistributedApplication> StartupAppAsync()
        {
            var cancellationToken = TestContext.CurrentContext.CancellationToken;

            var appHost =
                await DistributedApplicationTestingBuilder.CreateAsync<Projects.AspireApp_AppHost>(
                    cancellationToken
                );
            appHost.Services.AddLogging(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Debug);
                // Override the logging filters from the app's configuration
                logging.AddFilter(appHost.Environment.ApplicationName, LogLevel.Debug);
                logging.AddFilter("Aspire.", LogLevel.Debug);
            });
            appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
            {
                clientBuilder.AddStandardResilienceHandler();
            });

            var app = await appHost
                .BuildAsync(cancellationToken)
                .WaitAsync(DefaultTimeout, cancellationToken);
            await app.StartAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);

            return app;
        }

        public void Dispose()
        {
            _application.Dispose();
        }
    }
}
