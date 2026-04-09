namespace starter_code.Dtos
{
    // Data Transfer Object used when creating or updating a comment
    public class CommentDto
    {
        // The content of the comment submitted by the user
        public string Content { get; set; } = string.Empty;
    }
}