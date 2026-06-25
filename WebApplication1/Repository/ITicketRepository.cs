using WebApplication1.DTOs;
using WebApplication1.Modello;

namespace WebApplication1.Repositories
{
    public interface ITicketRepository
    {
        Task<List<Ticket>> GetAllByUserIdAsync(string userId);
        Task<Ticket?> GetByIdAsync(int id);
        Task<Ticket> AddAsync(Ticket ticket);
        Task<int> CountOpenTicketsByOperatorIdAsync(string operatorId);
        Task SaveChangesAsync();
        
    }
}