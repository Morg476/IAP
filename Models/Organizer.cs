namespace starter_code.Models
{
    // Represents an event organizer responsible for hosting events
    public class Organizer
    {
        // Primary key for the organizer
        public int Id { get; set; }

        // The organizer's name
        public string Name { get; set; } = string.Empty;

        // The organizer's email address (optional)
        public string? Email { get; set; }

        // The organizer's contact phone number (optional)
        public string? Phone { get; set; }

        // Navigation property for events managed by the organizer
        public ICollection<Event>? Events { get; set; }
    }
}