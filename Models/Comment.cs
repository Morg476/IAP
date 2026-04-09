namespace starter_code.Models
{
    // Represents a user comment on an event
    public class Comment
    {
        // Primary key for the comment
        public int Id { get; set; }

        // The content of the comment
        public string Content { get; set; } = string.Empty;

        // The date and time the comment was created
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key linking the comment to an event
        public int EventId { get; set; }

        // Navigation property for the related event
        public Event? Event { get; set; }

        // Foreign key linking the comment to a user
        public int UserId { get; set; }

        // Navigation property for the user who posted the comment
        public User? User { get; set; }
    }
}