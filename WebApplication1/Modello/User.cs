using Microsoft.AspNetCore.Identity;


namespace WebApplication1.Modello
{
    public class User : IdentityUser

    {
 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

        public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();

        public ICollection<OperatorSkill> Skills { get; set; } = new List<OperatorSkill>();
    }
}
