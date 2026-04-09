using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using starter_code.Data;
using starter_code.Dtos;
using starter_code.Models;
using System.Security.Claims;

namespace starter_code.Controllers
{
    [ApiController]
    [Route("api/v2/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly EventAppDbContext _context;

        // Inject the database context
        public CommentsController(EventAppDbContext context)
        {
            _context = context;
        }

        // Retrieve all comments for a specific event
        // GET: /api/v2/comments/{eventId}
        [HttpGet("{eventId:int}")]
        public async Task<IActionResult> GetAllForEvent(int eventId)
        {
            var eventExists = await _context.Events.AnyAsync(e => e.Id == eventId);
            if (!eventExists)
                return NotFound(new { message = "Event not found." });

            var comments = await _context.Comments
                .Where(c => c.EventId == eventId)
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new
                {
                    c.Id,
                    c.Content,
                    c.CreatedAt,
                    c.EventId,
                    c.UserId,
                    UserName = c.User != null ? c.User.Name : null
                })
                .ToListAsync();

            return Ok(comments);
        }

        // Retrieve a single comment for an event
        // GET: /api/v2/comments/{eventId}/{commentId}
        [HttpGet("{eventId:int}/{commentId:int}")]
        public async Task<IActionResult> GetOne(int eventId, int commentId)
        {
            var comment = await _context.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.EventId == eventId && c.Id == commentId);

            if (comment == null)
                return NotFound(new { message = "Comment not found." });

            return Ok(new
            {
                comment.Id,
                comment.Content,
                comment.CreatedAt,
                comment.EventId,
                comment.UserId,
                UserName = comment.User != null ? comment.User.Name : null
            });
        }

        // Create a new comment for an event
        // Only logged-in users and admins can post comments
        // POST: /api/v2/comments/{eventId}
        [Authorize(Roles = "User,Admin")]
        [HttpPost("{eventId:int}")]
        public async Task<IActionResult> Create(int eventId, [FromBody] CommentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
                return BadRequest(new { message = "Comment content is required." });

            var eventExists = await _context.Events.AnyAsync(e => e.Id == eventId);
            if (!eventExists)
                return NotFound(new { message = "Event not found." });

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "Invalid user token." });

            var comment = new Comment
            {
                Content = dto.Content,
                EventId = eventId,
                UserId = int.Parse(userIdClaim),
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOne),
                new { eventId, commentId = comment.Id }, comment);
        }

        // Update an existing comment
        // Users can edit their own comments, admins can edit any
        // PUT: /api/v2/comments/{eventId}/{commentId}
        [Authorize(Roles = "User,Admin")]
        [HttpPut("{eventId:int}/{commentId:int}")]
        public async Task<IActionResult> Update(int eventId, int commentId, [FromBody] CommentDto dto)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.EventId == eventId && c.Id == commentId);

            if (comment == null)
                return NotFound(new { message = "Comment not found." });

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "Invalid user token." });

            var userId = int.Parse(userIdClaim);

            // Ensure only the owner or an admin can update the comment
            if (role != "Admin" && comment.UserId != userId)
                return Forbid();

            comment.Content = dto.Content;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Comment updated successfully.", comment });
        }

        // Delete a comment
        // Users can delete their own comments, admins can delete any
        // DELETE: /api/v2/comments/{eventId}/{commentId}
        [Authorize(Roles = "User,Admin")]
        [HttpDelete("{eventId:int}/{commentId:int}")]
        public async Task<IActionResult> Delete(int eventId, int commentId)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.EventId == eventId && c.Id == commentId);

            if (comment == null)
                return NotFound(new { message = "Comment not found." });

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "Invalid user token." });

            var userId = int.Parse(userIdClaim);

            // Ensure only the owner or an admin can delete the comment
            if (role != "Admin" && comment.UserId != userId)
                return Forbid();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Comment deleted successfully." });
        }
    }
}