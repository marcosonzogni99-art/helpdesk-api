using System.ComponentModel.DataAnnotations;
using WebApplication1.Modello;

namespace WebApplication1.DTOs
{
    public class AddSkillDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public Category Category { get; set; }
    }
}