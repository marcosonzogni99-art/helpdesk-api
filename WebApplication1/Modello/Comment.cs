namespace WebApplication1.Modello
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int TicketId { get; set; }
        public Ticket Ticket { get; set; } = null!;
    }
}
