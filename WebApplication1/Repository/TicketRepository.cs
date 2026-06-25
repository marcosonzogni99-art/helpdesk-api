using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Modello;
using System.Linq;



namespace WebApplication1.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _db;

        public TicketRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Ticket>> GetAllByUserIdAsync(string userId)
        {
            return await _db.Tickets
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _db.Tickets
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Ticket> AddAsync(Ticket ticket)
        {
            _db.Tickets.Add(ticket);
            return ticket;
        }

        public async Task<int> CountOpenTicketsByOperatorIdAsync(string operatorId)
        {
            return await _db.Tickets
                .Where(t => t.OperatorId == operatorId && !t.IsResolved)
                .CountAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }



    }
}
