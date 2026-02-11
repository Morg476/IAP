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

        // READ ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _context.Messages.AsNoTracking().ToListAsync());

        // READ ONE
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var message = await _context.Messages.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            return message is null ? NotFound() : Ok(message);
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Message message)
        {
            message.Id = null; // let SQLite autogenerate
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOne), new { id = message.Id }, message);
        }

        // UPDATE
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateMessage(int id, [FromBody] Message updated)
        {
            var existing = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            if (existing is null) return NotFound();

            existing.Name = updated.Name;
            existing.Email = updated.Email;
            existing.Phone = updated.Phone;
            existing.Content = updated.Content;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        // DELETE
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var existing = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            if (existing is null) return NotFound();

            _context.Messages.Remove(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
