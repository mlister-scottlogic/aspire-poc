using AspireApp.ApiService.Contracts;
using Reqnroll;

namespace AspireApp.Tests.Features.StepDefinitions
{
    [Binding]
    internal class EntrySteps
    {
        private ScenarioContext _scenarioContext;

        public EntrySteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("some request")]
        public void GivenSomeRequest()
        {
            var request = new DailyEntry()
            {
                Title = "Day 1",
                Description = "I ate the porridge. It was too salty.",
                Date = DateOnly.Parse("2025-01-01"),
                Distance = 10.2m,
                DistanceUnit = DistanceUnit.Kilometers,
            };

            _scenarioContext["request"] = request;
        }

        [When("the entries request is sent")]
        public void WhenTheEntriesRequestIsSent()
        {
            var request = _scenarioContext.Get<DailyEntry>("request");
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
