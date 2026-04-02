namespace starter_code.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; } = "";

        public int EventId { get; set; }
        public Event? Event { get; set; }

        public int UserId { get; set; }
    }
}