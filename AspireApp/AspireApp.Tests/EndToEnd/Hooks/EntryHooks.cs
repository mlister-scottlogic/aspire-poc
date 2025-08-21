using AspireApp.Constants;
using AspireApp.Tests.EndToEnd.StepDefinitions;
using Reqnroll;

namespace AspireApp.Tests.EndToEnd.Hooks
{
    [Binding]
    internal class EntryHooks(ScenarioContext scenarioContext)
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(60);

        [BeforeScenario]
        public async Task BeforeScenario()
        {
            if (AppInstance.App is null)
            {
                throw new InvalidOperationException();
            }

            // Check app is healthy
            var cancellationToken = TestContext.CurrentContext.CancellationToken;

            var httpClient = AppInstance.App.CreateHttpClient(AspireConstants.Api);

            await AppInstance
                .App.ResourceNotifications.WaitForResourceHealthyAsync(
                    AspireConstants.Api,
                    cancellationToken
                )
                .WaitAsync(DefaultTimeout, cancellationToken);

            Console.WriteLine("App healthy");

            // Setup Scenario Context
            scenarioContext.SetHttpClient(httpClient);
        }
    }
}
