using WebApplication1.DTOs;

namespace WebApplication1.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string? Token, string? Error)> RegisterAsync(RegisterRequestDTO dto);
        Task<(bool Success, string? Token, string? Error)> LoginAsync(LoginRequestDTO dto);
        Task<bool> PromoteToOperatorAsync(string username);
        Task<bool> PromoteToSupervisorAsync(string username);

        Task<bool> AddSkillAsync(AddSkillDTO dto);
    }
}