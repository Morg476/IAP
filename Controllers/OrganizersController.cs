using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using starter_code.Data;
using starter_code.Models;
using starter_code.Dtos;

namespace starter_code.Controllers
{
    [ApiController]
    [Route("api/v2/organizers")]
    public class OrganizersController : ControllerBase
    {
        private readonly EventAppDbContext _context;

        public OrganizersController(EventAppDbContext context)
        {
            _context = context;
        }

        // GET: /api/v2/organizers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var organizers = await _context.Organizers
                .ToListAsync();

            return Ok(organizers);
        }

        // GET: /api/v2/organizers/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var organizer = await _context.Organizers
                .FirstOrDefaultAsync(o => o.Id == id);

            if (organizer == null)
                return NotFound(new { message = "Organizer not found." });

            return Ok(organizer);
        }

        // POST: /api/v2/organizers
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrganizerDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { message = "Organizer name is required." });

            var organizer = new Organizer
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone
            };

            _context.Organizers.Add(organizer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOne), new { id = organizer.Id }, organizer);
        }

        // PUT: /api/v2/organizers/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrganizerDto dto)
        {
            var organizer = await _context.Organizers.FindAsync(id);

            if (organizer == null)
                return NotFound(new { message = "Organizer not found." });

            organizer.Name = dto.Name;
            organizer.Email = dto.Email;
            organizer.Phone = dto.Phone;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Organizer updated successfully.", organizer });
        }

        // DELETE: /api/v2/organizers/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var organizer = await _context.Organizers.FindAsync(id);

            if (organizer == null)
                return NotFound(new { message = "Organizer not found." });

            _context.Organizers.Remove(organizer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Organizer deleted successfully." });
        }
    }
}