using Microsoft.AspNetCore.Mvc;

namespace starter_code.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class EventsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok("List of events");
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOne(int id)
        {
            return Ok($"Event with id {id}");
        }

        [HttpPost]
        public IActionResult Create([FromBody] string eventName)
        {
            return Created("", eventName);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] string eventName)
        {
            return Ok($"Event {id} updated");
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            return NoContent();
        }

        [HttpGet("search")]
        public IActionResult Search(string type, string term)
        {
            return Ok($"Search events by type={type} and term={term}");
        }
    }

    
}
