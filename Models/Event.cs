namespace starter_code.Models
{
    public class Event
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";
        public string Category { get; set; } = "";
        public string Location { get; set; } = "";

        // relationships
        public int? OrganizerId { get; set; }
        public Organizer? Organizer { get; set; }

        public ICollection<Booking>? Bookings { get; set; }

        public ICollection<Comment>? Comments { get; set; }


    }
}