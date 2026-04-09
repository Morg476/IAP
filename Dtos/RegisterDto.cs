namespace starter_code.Dtos
{
    // Data Transfer Object used when registering a new user
    public class RegisterDto
    {
        // The user's full name
        public string Name { get; set; } = "";

        // The user's email address used for login
        public string Email { get; set; } = "";

        // The user's password, which will be hashed before storage
        public string Password { get; set; } = "";
    }
}