using WebApplication1.DTOs;

namespace WebApplication1.Services
{
    public interface ITicketService
    {
        Task<List<TicketResponseDTO>> GetTicketsForUserAsync(string userId);
        Task<TicketResponseDTO?> GetTicketByIdAsync(int id, string userId);
        Task ImportTicketsForUserAsync(string userId);
        Task<bool> ResolveTicketAsync(int id, string userId);
        Task<TicketResponseDTO> CreateTicketWithAutoAssignAsync(string customerId, CreateTicketDTO dto);
    }
}