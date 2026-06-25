using System.ComponentModel.DataAnnotations;
using WebApplication1.Modello;

namespace WebApplication1.DTOs
{
    public class CreateTicketDTO
    {
        [Required(ErrorMessage = "Il titolo è obbligatorio")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descrizione è obbligatoria")]
        public string Body { get; set; } = string.Empty;

        [Required(ErrorMessage = "La categoria è obbligatoria")]
        public Category Category { get; set; }
    }
}
