namespace starter_code.Dtos
{
    // Data Transfer Object used when creating a new booking
    public class CreateBookingDto
    {
        // The ID of the event the user wants to book
        public int EventId { get; set; }
    }
}