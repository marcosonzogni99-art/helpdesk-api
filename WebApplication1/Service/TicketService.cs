using Microsoft.AspNetCore.Identity;
using System.Net.Sockets;
using WebApplication1.DTOs;

using WebApplication1.Modello;

using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IOperatorSkillRepository _skillRepository;
        private readonly HttpClient _httpClient;
        private readonly UserManager<User> _userManager;

        public TicketService(
            ITicketRepository ticketRepository,
            IOperatorSkillRepository skillRepository,
            HttpClient httpClient,
            UserManager<User> userManager)
        {
            _ticketRepository = ticketRepository;
            _skillRepository = skillRepository;
            _httpClient = httpClient;
            _userManager = userManager;
        }

        public async Task<List<TicketResponseDTO>> GetTicketsForUserAsync(string userId)
        {
            var tickets = await _ticketRepository.GetAllByUserIdAsync(userId);

            return tickets.Select(t => new TicketResponseDTO
            {
                Id = t.Id,
                Title = t.Title,
                Body = t.Body,
                IsResolved = t.IsResolved,
                ImportedAt = t.ImportedAt,
                Category = t.Category,      // ← aggiunto
                OperatorId = t.OperatorId   // ← aggiunto
                
        }).ToList();
        }

        public async Task<TicketResponseDTO?> GetTicketByIdAsync(int id, string userId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);

            if (ticket == null || ticket.UserId != userId)
                return null;

            return new TicketResponseDTO
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Body = ticket.Body,
                IsResolved = ticket.IsResolved,
                ImportedAt = ticket.ImportedAt,
                Category = ticket.Category,      // ← aggiunto
                OperatorId = ticket.OperatorId   // ← aggiunto
            };
        }

        public async Task ImportTicketsForUserAsync(string userId)
        {
            var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts?_limit=5");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var externalTickets = System.Text.Json.JsonSerializer.Deserialize<List<ExternalPost>>(json, options);

            if (externalTickets == null) return;

            foreach (var ext in externalTickets)
            {
                var ticket = new Ticket
                {
                    Title = ext.Title,
                    Body = ext.Body,
                    UserId = userId,
                    IsResolved = false,
                    ImportedAt = DateTime.UtcNow
                };

                await _ticketRepository.AddAsync(ticket);
            }

            await _ticketRepository.SaveChangesAsync();
        }

        public async Task<bool> ResolveTicketAsync(int id, string userId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);

            if (ticket == null || ticket.UserId != userId)
                return false;

            ticket.IsResolved = true;
            await _ticketRepository.SaveChangesAsync();
            return true;
        }


        public async Task<TicketResponseDTO> CreateTicketWithAutoAssignAsync(string customerId, CreateTicketDTO dto)
        {
            // 1. Trova tutti gli operatori con la competenza richiesta
            var eligibleOperatorIds = await _skillRepository.GetOperatorIdsByCategoryAsync(dto.Category);

            string? chosenOperatorId = null;

            if (eligibleOperatorIds.Any())
            {
                // 2. Tra questi, trova quello con MENO ticket aperti (load balancing)
                int minLoad = int.MaxValue;

                foreach (var operatorId in eligibleOperatorIds)
                {
                    var load = await _ticketRepository.CountOpenTicketsByOperatorIdAsync(operatorId);
                    if (load < minLoad)
                    {
                        minLoad = load;
                        chosenOperatorId = operatorId;
                    }
                }
            }

            // 3. Crea il ticket (assegnato se trovato un operatore, altrimenti "non assegnato")
            var ticket = new Ticket
            {
                Title = dto.Title,
                Body = dto.Body,
                Category = dto.Category,
                UserId = customerId,
                OperatorId = chosenOperatorId, // null se nessun operatore disponibile
                IsResolved = false,
                ImportedAt = DateTime.UtcNow
            };

            await _ticketRepository.AddAsync(ticket);
            await _ticketRepository.SaveChangesAsync();

            return new TicketResponseDTO
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Body = ticket.Body,
                IsResolved = ticket.IsResolved,
                ImportedAt = ticket.ImportedAt,
                Category = ticket.Category,      // ← aggiunto
                OperatorId = ticket.OperatorId   // ← aggiunto
            };
        }
}
}
    // Classe ausiliaria per deserializzare la risposta di JSONPlaceholder
    public class ExternalPost
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
