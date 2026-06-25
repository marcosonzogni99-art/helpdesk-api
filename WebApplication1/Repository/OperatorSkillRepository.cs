using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Modello;

namespace WebApplication1.Repositories
{
    public class OperatorSkillRepository : IOperatorSkillRepository
    {
        private readonly AppDbContext _db;

        public OperatorSkillRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<string>> GetOperatorIdsByCategoryAsync(Category category)
        {
            return await _db.OperatorSkills
                .Where(s => s.Category == category)
                .Select(s => s.UserId)
                .ToListAsync();
        }

        public async Task AddAsync(OperatorSkill skill)
        {
            _db.OperatorSkills.Add(skill);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}