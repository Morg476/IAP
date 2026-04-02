namespace starter_code.Models
{
    public class Organizer
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public List<Event> Events { get; set; } = new();
    }
}