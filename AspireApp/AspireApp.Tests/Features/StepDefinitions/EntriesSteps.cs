using Reqnroll;

namespace AspireApp.Tests.Features.StepDefinitions
{
    [Binding]
    internal class EntriesSteps
    {
        [Given("some request")]
        public void GivenSomeRequest()
        {
            throw new PendingStepException();
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
