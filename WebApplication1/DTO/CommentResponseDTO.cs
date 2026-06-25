namespace WebApplication1.DTOs
{
    public class CommentResponseDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}