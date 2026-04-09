using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using starter_code.Data;
using starter_code.Models;

namespace starter_code.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly EventAppDbContext _context;

        // Inject the database context
        public EventsController(EventAppDbContext context)
        {
            _context = context;
        }

        // Retrieve all events with their comments and organizers
        // GET: /api/v2/events
        [HttpGet]
        public IActionResult GetAll()
        {
            var events = _context.Events
                .Include(e => e.Comments)
                .Include(e => e.Organizer)
                .ToList();

            return Ok(events);
        }

        // Retrieve a single event by ID with related data
        // GET: /api/v2/events/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetOne(int id)
        {
            var ev = _context.Events
                .Include(e => e.Comments)
                .Include(e => e.Organizer)
                .FirstOrDefault(e => e.Id == id);

            if (ev == null) return NotFound();

            return Ok(ev);
        }

        // Create a new event (Admin only)
        // POST: /api/v2/events
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create([FromBody] Event dto)
        {
            if (dto == null) return BadRequest();

            _context.Events.Add(dto);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetOne), new { id = dto.Id }, dto);
        }

        // Update an existing event (Admin only)
        // PUT: /api/v2/events/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] Event updated)
        {
            var existing = _context.Events.FirstOrDefault(e => e.Id == id);
            if (existing == null) return NotFound();

            existing.Title = updated.Title;
            existing.Category = updated.Category;
            existing.Location = updated.Location;
            existing.OrganizerId = updated.OrganizerId;

            _context.SaveChanges();

            return Ok(existing);
        }

        // Delete an event (Admin only)
        // DELETE: /api/v2/events/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var existing = _context.Events.FirstOrDefault(e => e.Id == id);
            if (existing == null) return NotFound();

            _context.Events.Remove(existing);
            _context.SaveChanges();

            return NoContent();
        }

        // Search for events using one or more query parameters
        // GET: /api/v2/events/search?title=&category=&location=
        [HttpGet("search")]
        public IActionResult Search(
            [FromQuery] string? title,
            [FromQuery] string? category,
            [FromQuery] string? location)
        {
            var query = _context.Events.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(e => e.Title.Contains(title));

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(e => e.Category.Contains(category));

            if (!string.IsNullOrWhiteSpace(location))
                query = query.Where(e => e.Location.Contains(location));

            return Ok(query.ToList());
        }
    }
}