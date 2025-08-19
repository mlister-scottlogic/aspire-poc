using AspireApp.ApiService.Contracts;
using Reqnroll;
using Shouldly;
using System.Net.Http.Json;

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

        [Given("an entries request")]
        public void GivenAnEntriesRequest(DataTable dataTable)
        {
            var dailyEntryData = dataTable.CreateInstance<DailyEntryTable>();

            var request = MapTableToContract(dailyEntryData);

            _scenarioContext.SetRequest(request);
        }

        private class DailyEntryTable
        {
            public string? Title { get; set; }

            public string? Description { get; set; }

            public string? Date { get; set; }

            public decimal? Distance { get; set; }

            public DistanceUnit? DistanceUnit { get; set; }
        }

        private static DailyEntry MapTableToContract(DailyEntryTable dailyEntryTable)
        {
            return new DailyEntry()
            {
                Title = dailyEntryTable.Title,
                Description = dailyEntryTable.Description,
                Date = dailyEntryTable.Date != null ? DateOnly.Parse(dailyEntryTable.Date!) : null,
                Distance = dailyEntryTable.Distance,
                DistanceUnit = dailyEntryTable.DistanceUnit,
            };
        }

        [When("the entries request is sent")]
        public async Task WhenTheEntriesRequestIsSent()
        {
            var request = _scenarioContext.GetRequest();

            var httpClient = _scenarioContext.GetHttpClient();

            var response = await httpClient.PostAsJsonAsync("/entries", request);

            _scenarioContext.SetResponse(response);
        }

        [Then("the entries request is successful with a status code of {int}")]
        public void ThenTheEntriesRequestIsSuccessfulWithAStatusCodeOf(int statusCode)
        {
            var response = _scenarioContext.GetResponse();

            response.IsSuccessStatusCode.ShouldBeTrue();

            ((int)response.StatusCode).ShouldBe(statusCode);
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
