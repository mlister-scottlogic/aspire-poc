using AspireApp.Tests.EndToEnd.StepDefinitions;
using Reqnroll;

namespace AspireApp.Tests.EndToEnd.Hooks
{
    [Binding]
    internal class EntryHooks
    {
        private ScenarioContext _scenarioContext;
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(60);

        public EntryHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public async Task BeforeScenario()
        {
            // Check app is healthy
            var cancellationToken = TestContext.CurrentContext.CancellationToken;

            var httpClient = AppInstance.App.CreateHttpClient("apiservice");

            await AppInstance
                .App.ResourceNotifications.WaitForResourceHealthyAsync(
                    "apiservice",
                    cancellationToken
                )
                .WaitAsync(DefaultTimeout, cancellationToken);

            Console.WriteLine("App healthy");

            // Setup Scenario Context
            _scenarioContext.SetHttpClient(httpClient);
        }
    }
}
