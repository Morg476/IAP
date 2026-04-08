namespace starter_code.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Event? Event { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public DateTime BookedAt { get; set; } = DateTime.UtcNow;

        public string TicketCode { get; set; } = string.Empty;
    }
}