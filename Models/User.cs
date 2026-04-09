namespace starter_code.Models;

// Represents a registered user of the system
public class User
{
    // Primary key for the user
    public int Id { get; set; }

    // The user's full name
    public string Name { get; set; } = string.Empty;

    // The user's email address used for login
    public string Email { get; set; } = string.Empty;

    // Stores the hashed password for security
    public string PasswordHash { get; set; } = string.Empty;

    // The user's role (e.g., User or Admin)
    public string Role { get; set; } = "User";

    // Navigation property for comments made by the user
    public ICollection<Comment>? Comments { get; set; }

    // Navigation property for bookings made by the user
    public ICollection<Booking>? Bookings { get; set; }
}