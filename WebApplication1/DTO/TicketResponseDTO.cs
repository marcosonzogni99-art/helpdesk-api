using WebApplication1.Modello;

namespace WebApplication1.DTOs
{
    public class TicketResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsResolved { get; set; }
        public DateTime ImportedAt { get; set; }
        public Category Category { get; set; }
        public string? OperatorId { get; set; }
    }
}