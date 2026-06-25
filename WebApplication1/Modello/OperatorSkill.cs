using WebApplication1.Modello;

namespace WebApplication1.Modello
{
    public class OperatorSkill
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;

        public Category Category { get; set; }
    }
}
