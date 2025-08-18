using AspireApp.Tests.Features.Hooks;
using Reqnroll;

namespace AspireApp.Tests.Features.StepDefinitions
{
    [Binding]
    internal class EntriesSteps
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(60);

        [Given("some request")]
        public async Task GivenSomeRequest()
        {
            var cancellationToken = TestContext.CurrentContext.CancellationToken;

            using var httpClient = ApiInstance.App.CreateHttpClient("apiservice");

            await ApiInstance
                .App.ResourceNotifications.WaitForResourceHealthyAsync(
                    "apiservice",
                    cancellationToken
                )
                .WaitAsync(DefaultTimeout, cancellationToken);

            Console.WriteLine("App healthy");
        }

        [When("the entries request is sent")]
        public void WhenTheEntriesRequestIsSent()
        {
            throw new PendingStepException();
        }

        [Then("the entries request is successful with a status code of {string}")]
        public void ThenTheEntriesRequestIsSuccessfulWithAStatusCodeOf(string p0)
        {
            throw new PendingStepException();
        }

        [Then("the data is stored in the database")]
        public void ThenTheDataIsStoredInTheDatabase()
        {
            throw new PendingStepException();
        }

        [Then("there is an event on the downstream queue")]
        public void ThenThereIsAnEventOnTheDownstreamQueue()
        {
            throw new PendingStepException();
        }
    }
}
