using WebApplication1.DTOs;
using WebApplication1.Modello;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<List<CommentResponseDTO>> GetCommentsForTicketAsync(int ticketId)
        {
            var comments = await _commentRepository.GetAllByTicketIdAsync(ticketId);

            return comments.Select(c => new CommentResponseDTO
            {
                Id = c.Id,
                Text = c.Text,
                CreatedAt = c.CreatedAt
            }).ToList();
        }

        public async Task AddCommentAsync(int ticketId, CommentRequestDTO dto)
        {
            var comment = new Comment
            {
                Text = dto.Text,
                TicketId = ticketId,
                CreatedAt = DateTime.UtcNow
            };

            await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveChangesAsync();
        }
    }
}