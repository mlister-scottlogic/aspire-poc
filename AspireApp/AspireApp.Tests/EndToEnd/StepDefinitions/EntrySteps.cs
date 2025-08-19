using AspireApp.ApiService.Contracts;
using Reqnroll;
using Shouldly;
using System.Net.Http.Json;

namespace AspireApp.Tests.EndToEnd.StepDefinitions
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

        public class DailyEntryTable
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
        public async Task ThenTheEntriesRequestIsSuccessfulWithAStatusCodeOf(int statusCode)
        {
            var response = _scenarioContext.GetResponse();

            response.IsSuccessStatusCode.ShouldBeTrue();

            ((int)response.StatusCode).ShouldBe(statusCode);

            var dailyEntriesResponse = await response.Content.ReadFromJsonAsync<DailyEntryWithId>();
            _scenarioContext.SetEntryId(dailyEntriesResponse!.Id);
        }

        [Then("the entry data is stored in the database")]
        public async Task ThenTheDataIsStoredInTheDatabase(DataTable dataTable)
        {
            var dailyEntryData = dataTable.CreateInstance<DailyEntryTable>();

            var entryId = _scenarioContext.GetEntryId();

            entryId.ShouldNotBeNull();

            var repo = await AppInstance.GetDailyEntryRepositoryAsync();
            var data = await repo.GetDailyEntryByIdAsync(entryId.Value);

            // Get by entryId from database
            // Assert against data table
        }

        [Then("there is an event on the downstream queue")]
        public void ThenThereIsAnEventOnTheDownstreamQueue()
        {
            throw new PendingStepException();
        }
    }
}
