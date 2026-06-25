using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ICommentService _commentService;

        public TicketsController(ITicketService ticketService, ICommentService commentService)
        {
            _ticketService = ticketService;
            _commentService = commentService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyTickets()
        {
            var userId = GetUserId();
            var tickets = await _ticketService.GetTicketsForUserAsync(userId);
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            var userId = GetUserId();
            var ticket = await _ticketService.GetTicketByIdAsync(id, userId);

            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }

        [HttpPatch("{id}/resolve")]
        public async Task<IActionResult> ResolveTicket(int id)
        {
            var userId = GetUserId();
            var success = await _ticketService.ResolveTicketAsync(id, userId);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetComments(int id)
        {
            var comments = await _commentService.GetCommentsForTicketAsync(id);
            return Ok(comments);
        }

        [HttpPost("{id}/comments")]
        public async Task<IActionResult> AddComment(int id, [FromBody] CommentRequestDTO dto)
        {
            await _commentService.AddCommentAsync(id, dto);
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDTO dto)
        {
            var customerId = GetUserId();
            var ticket = await _ticketService.CreateTicketWithAutoAssignAsync(customerId, dto);
            return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, ticket);
        }
    }
}