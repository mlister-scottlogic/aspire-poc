using AspireApp.ApiService.Contracts.Enums;
using Microsoft.AspNetCore.Mvc;

namespace AspireApp.ApiService.Web.Controllers
{
    [Route("entries")]
    [ApiController]
    public class EntryController : Controller
    {
        [HttpPost]
        public IActionResult Post([FromBody] DailyEntry entry)
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            return Ok();
        }
    }
}
