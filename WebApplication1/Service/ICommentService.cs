using WebApplication1.DTOs;
using WebApplication1.Modello;

namespace WebApplication1.Services
{
    public interface ICommentService
    {
        Task<List<CommentResponseDTO>> GetCommentsForTicketAsync(int ticketId);
        Task AddCommentAsync(int ticketId, CommentRequestDTO dto);
    }
}