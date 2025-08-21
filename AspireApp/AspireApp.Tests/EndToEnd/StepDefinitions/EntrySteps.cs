using System.Net.Http.Json;
using AspireApp.ApiService.Contracts;
using AspireApp.Tests.EndToEnd.Database;
using Reqnroll;
using Shouldly;

namespace AspireApp.Tests.EndToEnd.StepDefinitions
{
    [Binding]
    internal class EntrySteps(ScenarioContext scenarioContext)
    {
        [Given("an entries request")]
        public void GivenAnEntriesRequest(DataTable dataTable)
        {
            var dailyEntryData = dataTable.CreateInstance<DailyEntryTable>();

            var request = MapTableToContract(dailyEntryData);

            scenarioContext.SetRequest(request);
        }

        [When("the entries request is sent")]
        public async Task WhenTheEntriesRequestIsSent()
        {
            var request = scenarioContext.GetRequest();

            var httpClient = scenarioContext.GetHttpClient();

            var response = await httpClient.PostAsJsonAsync("/entries", request);

            scenarioContext.SetResponse(response);
        }

        [Then("the entries request is successful with a status code of {int}")]
        public async Task ThenTheEntriesRequestIsSuccessfulWithAStatusCodeOf(int statusCode)
        {
            var response = scenarioContext.GetResponse();

            response.IsSuccessStatusCode.ShouldBeTrue();

            ((int)response.StatusCode).ShouldBe(statusCode);

            var dailyEntriesResponse = await response.Content.ReadFromJsonAsync<DailyEntryWithId>();
            scenarioContext.SetEntryId(dailyEntriesResponse!.Id);
        }

        [Then("the entries request is unsuccessful with a status code of {int}")]
        public void ThenTheEntriesRequestIsUnsuccessfulWithAStatusCodeOf(int statusCode)
        {
            var response = scenarioContext.GetResponse();

            response.IsSuccessStatusCode.ShouldBeFalse();

            ((int)response.StatusCode).ShouldBe(statusCode);
        }

        [Then("the entry data is stored in the database")]
        public async Task ThenTheDataIsStoredInTheDatabase(DataTable dataTable)
        {
            var entryId = scenarioContext.GetEntryId();

            entryId.ShouldNotBeNull();

            var repo = await AppInstance.GetDailyEntryRepositoryAsync();
            var data = await repo.GetDailyEntryByIdAsync(entryId.Value);

            data.ShouldNotBeNull();

            var dataTableEquivalent = MapDatabaseToTable(data);
            dataTable.CompareToInstance(dataTableEquivalent);
        }

        [Then("there is an event on the entries queue")]
        public void ThenThereIsAnEventOnTheDownstreamQueue(DataTable dataTable)
        {
            var entryId = scenarioContext.GetEntryId();

            // Wait for message to arrive on queue (could be a while)
            // Assert message has same details as dataTable

            // Failure until implemented
            true.ShouldBeFalse();
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

        private static DailyEntryTable MapDatabaseToTable(DailyEntryEntity entity)
        {
            var parsed = Enum.TryParse<DistanceUnit>(entity.DistanceUnit, out var distanceUnit);

            return new DailyEntryTable()
            {
                Title = entity.Title,
                Description = entity.Description,
                Date = DateOnly.FromDateTime(entity.Date).ToString("yyyy-MM-dd"),
                Distance = entity.Distance,
                DistanceUnit = parsed ? distanceUnit : null,
            };
        }
    }
}
