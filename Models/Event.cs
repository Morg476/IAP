namespace starter_code.Models
{
    // Represents an event that users can view, comment on, and book
    public class Event
    {
        // Primary key for the event
        public int Id { get; set; }

        // The title of the event
        public string Title { get; set; } = "";

        // The category or type of the event (e.g., Workshop, Meetup)
        public string Category { get; set; } = "";

        // The location where the event takes place
        public string Location { get; set; } = "";

        // Foreign key linking the event to an organizer (optional)
        public int? OrganizerId { get; set; }

        // Navigation property for the related organizer
        public Organizer? Organizer { get; set; }

        // Navigation property for bookings associated with the event
        public ICollection<Booking>? Bookings { get; set; }

        // Navigation property for comments associated with the event
        public ICollection<Comment>? Comments { get; set; }
    }
}