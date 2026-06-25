using WebApplication1.Modello;


namespace WebApplication1.Repositories
{
    public interface IOperatorSkillRepository
    {
        Task<List<string>> GetOperatorIdsByCategoryAsync(Category category);
        Task AddAsync(OperatorSkill skill);
        Task SaveChangesAsync();
    }
}