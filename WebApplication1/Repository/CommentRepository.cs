using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Modello;

namespace WebApplication1.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _db;

        public CommentRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Comment>> GetAllByTicketIdAsync(int ticketId)
        {
            return await _db.Comments
                .Where(c => c.TicketId == ticketId)
                .ToListAsync();
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            _db.Comments.Add(comment);
            return comment;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}