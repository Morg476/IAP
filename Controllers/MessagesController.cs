using Microsoft.AspNetCore.Mvc;
using starter_code.Models;

namespace starter_code.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class MessagesController : ControllerBase
    {
        // Simulated database with hardcoded data
        private static readonly List<Message> Messages = new()
        {
            new Message
            {
                Id = 1,
                Name = "Alice Brown",
                Email = "alice@example.com",
                Phone = "+44 1111 222333",
                Content = "I am interested in your training programs."
            },
            new Message
            {
                Id = 2,
                Name = "Bob Smith",
                Email = "bob@example.com",
                Content = "Please send more details about pricing."
            }
        };

        // READ ALL
        [HttpGet]
        public IActionResult GetAll() => Ok(Messages);

        // READ ONE
        [HttpGet("{id:int}")]
        public IActionResult GetOne(int id)
        {
            var message = Messages.FirstOrDefault(m => m.Id == id);
            return message is null ? NotFound() : Ok(message);
        }

        // CREATE
        [HttpPost]
        public IActionResult Create([FromBody] Message message)
        {
            message.Id = Messages.Count + 1;
            Messages.Add(message);

            return CreatedAtAction(nameof(GetOne), new { id = message.Id }, message);
        }

        // UPDATE (Full replacement)
        [HttpPut("{id:int}")]
        public IActionResult UpdateMessage(int id, [FromBody] Message updatedMessage)
        {
            var existing = Messages.FirstOrDefault(m => m.Id == id);
            if (existing is null) return NotFound();

            existing.Name = updatedMessage.Name;
            existing.Email = updatedMessage.Email;
            existing.Phone = updatedMessage.Phone;
            existing.Content = updatedMessage.Content;

            return Ok(existing);
        }

        // DELETE
        [HttpDelete("{id:int}")]
        public IActionResult DeleteMessage(int id)
        {
            var message = Messages.FirstOrDefault(m => m.Id == id);
            if (message is null) return NotFound();

            Messages.Remove(message);
            return NoContent();
        }
    }
}
