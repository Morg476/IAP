using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using starter_code.Data;
using starter_code.Models;

namespace starter_code.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly EventAppDbContext _context;

        public MessagesController(EventAppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return Unauthorized();

            if (!User.IsInRole("Admin"))
                return Forbid();

            return Ok(await _context.Messages.AsNoTracking().ToListAsync());
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return Unauthorized();

            if (!User.IsInRole("Admin"))
                return Forbid();

            var message = await _context.Messages.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            return message is null ? NotFound() : Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Message message)
        {
            message.Id = null;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOne), new { id = message.Id }, message);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateMessage(int id, [FromBody] Message updated)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return Unauthorized();

            if (!User.IsInRole("Admin"))
                return Forbid();

            var existing = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            if (existing is null) return NotFound();

            existing.Name = updated.Name;
            existing.Email = updated.Email;
            existing.Phone = updated.Phone;
            existing.Content = updated.Content;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return Unauthorized();

            if (!User.IsInRole("Admin"))
                return Forbid();

            var existing = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            if (existing is null) return NotFound();

            _context.Messages.Remove(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}