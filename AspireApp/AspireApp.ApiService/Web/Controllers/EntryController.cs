using AspireApp.ApiService.Contracts;
using AspireApp.ApiService.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspireApp.ApiService.Web.Controllers
{
    [Route("entries")]
    [ApiController]
    public class EntryController : Controller
    {
        private readonly IDailyEntryService _service;

        public EntryController(IDailyEntryService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] DailyEntry entry)
        {
            var result = await _service.AddEntryAsync(ToDomain(entry));

            return Ok(ToContract(result));
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var result = await _service.GetDailyEntry(id);

            return result.Match<IActionResult>(
                (entry) =>
                {
                    return Ok(ToContract(entry));
                },
                NotFound
            );
        }

        private static Domain.Models.DailyEntry ToDomain(DailyEntry entry)
        {
            // Validation means these properties now definitely have values
#pragma warning disable CS8629 // Nullable value type may be null.
            return new Domain.Models.DailyEntry()
            {
                Title = entry.Title!,
                Description = entry.Description,
                Date = entry.Date.Value,
                Distance = entry.Distance.Value,
                // Distance unit not required yet
                DistanceUnit = entry.DistanceUnit ?? Domain.Enums.DistanceUnit.Kilometers,
            };
#pragma warning restore CS8629 // Nullable value type may be null.
        }

        private static DailyEntryWithId ToContract(Domain.Models.DailyEntry entry)
        {
            return new DailyEntryWithId()
            {
                // Id is set by domain
#pragma warning disable CS8629 // Nullable value type may be null.
                Id = entry.Id.Value,
#pragma warning restore CS8629 // Nullable value type may be null.
                Title = entry.Title!,
                Description = entry.Description,
                Date = entry.Date,
                Distance = entry.Distance,
                DistanceUnit = entry.DistanceUnit,
            };
        }
    }
}
