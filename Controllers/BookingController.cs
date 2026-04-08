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
    [Route("api/v2/bookings")]
    public class BookingsController : ControllerBase
    {
        private readonly EventAppDbContext _context;

        public BookingsController(EventAppDbContext context)
        {
            _context = context;
        }

        // POST: /api/v2/bookings
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "Invalid user token." });

            var userId = int.Parse(userIdClaim);

            var ev = await _context.Events.FindAsync(dto.EventId);
            if (ev == null)
                return NotFound(new { message = "Event not found." });

            var alreadyBooked = await _context.Bookings
                .AnyAsync(b => b.UserId == userId && b.EventId == dto.EventId);

            if (alreadyBooked)
                return BadRequest(new { message = "You have already booked this event." });

            var booking = new Booking
            {
                EventId = dto.EventId,
                UserId = userId,
                BookedAt = DateTime.UtcNow,
                TicketCode = $"EVT-{dto.EventId}-USR-{userId}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMyBookings), new
            {
                id = booking.Id
            },
            new
            {
                message = "Booking created successfully.",
                booking.Id,
                booking.EventId,
                booking.UserId,
                booking.BookedAt,
                booking.TicketCode
            });
        }

        // GET: /api/v2/bookings/me
        [Authorize(Roles = "User,Admin")]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "Invalid user token." });

            var userId = int.Parse(userIdClaim);

            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Event)
                .OrderByDescending(b => b.BookedAt)
                .Select(b => new
                {
                    b.Id,
                    b.EventId,
                    EventTitle = b.Event != null ? b.Event.Title : null,
                    EventCategory = b.Event != null ? b.Event.Category : null,
                    EventLocation = b.Event != null ? b.Event.Location : null,
                    b.BookedAt,
                    b.TicketCode
                })
                .ToListAsync();

            return Ok(bookings);
        }

        // GET: /api/v2/bookings
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.User)
                .OrderByDescending(b => b.BookedAt)
                .Select(b => new
                {
                    b.Id,
                    b.EventId,
                    EventTitle = b.Event != null ? b.Event.Title : null,
                    b.UserId,
                    UserName = b.User != null ? b.User.Name : null,
                    UserEmail = b.User != null ? b.User.Email : null,
                    b.BookedAt,
                    b.TicketCode
                })
                .ToListAsync();

            return Ok(bookings);
        }

        // DELETE: /api/v2/bookings/5
        [Authorize(Roles = "User,Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "Invalid user token." });

            var userId = int.Parse(userIdClaim);

            if (role != "Admin" && booking.UserId != userId)
                return Forbid();

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking cancelled successfully." });
        }
    }
}