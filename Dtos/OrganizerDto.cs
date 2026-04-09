namespace starter_code.Dtos
{
    // Data Transfer Object used when creating or updating an organizer
    public class OrganizerDto
    {
        // The name of the event organizer
        public string Name { get; set; } = string.Empty;

        // The organizer's email address (optional)
        public string? Email { get; set; }

        // The organizer's contact phone number (optional)
        public string? Phone { get; set; }
    }
}