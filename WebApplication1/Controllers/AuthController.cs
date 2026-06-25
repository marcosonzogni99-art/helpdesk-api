using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (!result.Success)
                return BadRequest(new { error = result.Error });

            return Ok(new { token = result.Token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.Success)
                return Unauthorized(new { error = result.Error });

            return Ok(new { token = result.Token });
        }

        [HttpPost("promote-operator/{username}")]
        public async Task<IActionResult> PromoteToOperator(string username)
        {
            var result = await _authService.PromoteToOperatorAsync(username);

            if (!result)
                return NotFound(new { error = "Utente non trovato" });

            return Ok(new { message = $"{username} è ora Operator" });
        }

        [HttpPost("promote-supervisor/{username}")]
        public async Task<IActionResult> PromoteToSupervisor(string username)
        {
            var result = await _authService.PromoteToSupervisorAsync(username);

            if (!result)
                return NotFound(new { error = "Utente non trovato" });

            return Ok(new { message = $"{username} è ora Supervisor" });
        }

        [HttpPost("add-skill")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> AddSkill([FromBody] AddSkillDTO dto)
        {
            var result = await _authService.AddSkillAsync(dto);

            if (!result)
                return NotFound(new { error = "Operatore non trovato" });

            return Ok(new { message = $"Competenza {dto.Category} aggiunta a {dto.Username}" });
        }
    }
}