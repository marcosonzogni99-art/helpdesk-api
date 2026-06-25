using WebApplication1.Modello;

namespace WebApplication1.Modello
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsResolved { get; set; } = false;
        public DateTime ImportedAt { get; set; } = DateTime.UtcNow;
        public Category Category { get; set; }

        // Il Customer che ha aperto il ticket
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;

        // L'Operator assegnato (può essere null = non assegnato ancora)
        public string? OperatorId { get; set; }
        public User? Operator { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}