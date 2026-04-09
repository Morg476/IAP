namespace starter_code.Dtos;

// Data Transfer Object used for user authentication
public class LoginDto
{
    // The user's registered email address
    public string Email { get; set; } = string.Empty;

    // The user's password used during login
    public string Password { get; set; } = string.Empty;
}