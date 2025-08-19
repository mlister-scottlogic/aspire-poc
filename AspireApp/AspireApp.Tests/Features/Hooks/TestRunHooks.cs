using Microsoft.Extensions.Logging;
using Reqnroll;

namespace AspireApp.Tests.Features.Hooks
{
    [Binding]
    internal class TestRunHooks
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(60);

        [BeforeTestRun]
        public static async Task StartApiAsync()
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

            ApiInstance.App = app;
        }

        [AfterTestRun]
        public static async Task StopApiAsync()
        {
            if (ApiInstance.App != null)
            {
                await ApiInstance.App.DisposeAsync();
            }
        }
    }
}
