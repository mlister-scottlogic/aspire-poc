using AspireApp.ApiService.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspireApp.ApiService.Web.Controllers
{
    [Route("outbox-failures/entries")]
    [ApiController]
    public class DailyEntryOutboxErrorController : Controller
    {
        private readonly IOutboxAdminService<Domain.Models.DailyEntryWithId> _outboxAdminService;

        public DailyEntryOutboxErrorController(
            IOutboxAdminService<Domain.Models.DailyEntryWithId> outboxAdminService
        )
        {
            _outboxAdminService = outboxAdminService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var results = await _outboxAdminService.GetFailedMessagesAsync();

            return Ok(results.Select(ToContract).ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _outboxAdminService.GetFailedMessageAsync(id);

            return result.Match<IActionResult>((m) => Ok(ToContract(m)), NotFound);
        }

        [HttpPost("{id}/retry")]
        public async Task<IActionResult> RetryMessage([FromRoute] int id)
        {
            var result = await _outboxAdminService.RetryMessageAsync(id);

            if (result)
            {
                return Accepted();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById([FromRoute] int id)
        {
            var result = await _outboxAdminService.DeleteMessageAsync(id);

            if (result)
            {
                return Accepted();
            }

            return NotFound();
        }

        private Contracts.FailedOutboxMessage ToContract(Domain.Models.FailedOutboxMessage message)
        {
            return new Contracts.FailedOutboxMessage()
            {
                Id = message.Id,
                ProcessingAttempts = message.ProcessingAttempts,
                LinkedId = message.LinkedId,
            };
        }
    }
}
