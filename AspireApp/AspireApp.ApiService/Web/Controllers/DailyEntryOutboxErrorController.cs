using Microsoft.AspNetCore.Mvc;

namespace AspireApp.ApiService.Web.Controllers
{
    [Route("outbox-failures/entries")]
    [ApiController]
    public class DailyEntryOutboxErrorController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            return Ok();
        }

        [HttpPost("{id}/retry")]
        public IActionResult RetryMessage([FromRoute] int id)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById([FromRoute] int id)
        {
            return Ok();
        }
    }
}
