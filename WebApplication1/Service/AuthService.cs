using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.DTOs;
using WebApplication1.Modello;
using WebApplication1.Repositories;


namespace WebApplication1.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITicketService _ticketService;
        private readonly IOperatorSkillRepository _skillRepository;

        public AuthService(

            UserManager<User> userManager,
            IConfiguration configuration,
            ITicketService ticketService,
            IOperatorSkillRepository skillRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _ticketService = ticketService;
            _skillRepository = skillRepository;
        }

        public async Task<bool> AddSkillAsync(AddSkillDTO dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null) return false;

            if (!await _userManager.IsInRoleAsync(user, "Operator"))
                return false;

            var skill = new OperatorSkill
            {
                UserId = user.Id,
                Category = dto.Category
            };

            await _skillRepository.AddAsync(skill);
            await _skillRepository.SaveChangesAsync();
            return true;
        }

        

        public async Task<(bool Success, string? Token, string? Error)> RegisterAsync(RegisterRequestDTO dto)
        {
            var existingUser = await _userManager.FindByNameAsync(dto.Username);
            if (existingUser != null)
                return (false, null, "Username già in uso");

            var user = new User
            {
                UserName = dto.Username,
                Email = $"{dto.Username}@example.com"
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, null, errors);
            }

            await _userManager.AddToRoleAsync(user, "Customer");

            // Importa automaticamente i ticket per il nuovo utente
            await _ticketService.ImportTicketsForUserAsync(user.Id);

            var token = await GenerateJwtToken(user);
            return (true, token, null);
        }

        public async Task<(bool Success, string? Token, string? Error)> LoginAsync(LoginRequestDTO dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null)
                return (false, null, "Credenziali non valide");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordValid)
                return (false, null, "Credenziali non valide");

            var token = await GenerateJwtToken(user);
            return (true, token, null);
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
    };

            // Aggiungi i ruoli come claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<bool> PromoteToOperatorAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return false;

            if (!await _userManager.IsInRoleAsync(user, "Operator"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Customer");
                await _userManager.AddToRoleAsync(user, "Operator");
            }

            return true;
        }

        public async Task<bool> PromoteToSupervisorAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return false;

            if (!await _userManager.IsInRoleAsync(user, "Supervisor"))
            {
                await _userManager.AddToRoleAsync(user, "Supervisor");
            }

            return true;
        }

        

    }
}