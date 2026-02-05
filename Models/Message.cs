using System.ComponentModel.DataAnnotations;
using starter_code.Models;


namespace starter_code.Models
{
    public class Message
    {
        // Nullable on create (server-generated)
        public int? Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = String.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = String.Empty;

        // Optional field
        [Phone(ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Message must be at least 5 characters")]
        public string Content { get; set; } = String.Empty;
    }
}
