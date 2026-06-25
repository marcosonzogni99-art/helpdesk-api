using WebApplication1.Modello;

namespace WebApplication1.Repositories
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllByTicketIdAsync(int ticketId);
        Task<Comment> AddAsync(Comment comment);
        Task SaveChangesAsync();
    }
}
