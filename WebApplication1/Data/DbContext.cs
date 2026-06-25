using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Modello;



namespace WebApplication1.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public DbSet<OperatorSkill> OperatorSkills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relazione Ticket -> Customer (User)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relazione Ticket -> Operator (User)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Operator)
                .WithMany(u => u.AssignedTickets)
                .HasForeignKey(t => t.OperatorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}