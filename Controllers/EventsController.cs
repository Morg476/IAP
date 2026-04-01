using Microsoft.AspNetCore.Authorization;
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
            var events = new[]
            {
                new { Id = 1, Title = "Music Festival", Category = "Music", Location = "Central Park" },
                new { Id = 2, Title = "Tech Expo", Category = "Technology", Location = "City Hall" }
            };

            return Ok(events);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOne(int id)
        {
            var ev = new { Id = id, Title = "Sample Event", Category = "General", Location = "Central" };
            return Ok(ev);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create([FromBody] EventCreateDto dto)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return Unauthorized();

            if (!User.IsInRole("Admin"))
                return Forbid();

            if (dto == null)
                return BadRequest();

            return Created("/api/v2/events/1", dto);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] EventCreateDto dto)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return Unauthorized();

            if (!User.IsInRole("Admin"))
                return Forbid();

            if (dto == null)
                return BadRequest();

            var updatedEvent = new
            {
                Id = id,
                Title = dto.Title,
                Category = dto.Category,
                Location = dto.Location
            };

            return Ok(updatedEvent);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return Unauthorized();

            if (!User.IsInRole("Admin"))
                return Forbid();

            return NoContent();
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string? title, [FromQuery] string? category, [FromQuery] string? location)
        {
            var events = new[]
            {
                new { Id = 1, Title = "Music Festival", Category = "Music", Location = "Central Park" },
                new { Id = 2, Title = "Central Music Night", Category = "Music", Location = "Central" },
                new { Id = 3, Title = "Business Meetup", Category = "Business", Location = "Downtown" }
            };

            var results = events.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(title))
                results = results.Where(e => e.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(category))
                results = results.Where(e => e.Category.Contains(category, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(location))
                results = results.Where(e => e.Location.Contains(location, StringComparison.OrdinalIgnoreCase));

            return Ok(results);
        }
    }

    public class EventCreateDto
    {
        public string? Title { get; set; }
        public string? Category { get; set; }
        public string? Location { get; set; }
    }
}