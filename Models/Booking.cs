namespace starter_code.Models
{
    // Represents a user's booking for an event
    public class Booking
    {
        // Primary key for the booking
        public int Id { get; set; }

        // Foreign key linking to the booked event
        public int EventId { get; set; }

        // Navigation property for the related event
        public Event? Event { get; set; }

        // Foreign key linking to the user who made the booking
        public int UserId { get; set; }

        // Navigation property for the related user
        public User? User { get; set; }

        // The date and time the booking was created
        public DateTime BookedAt { get; set; } = DateTime.UtcNow;

        // Unique ticket code generated for the booking
        public string TicketCode { get; set; } = string.Empty;
    }
}