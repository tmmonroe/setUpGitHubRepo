using Microsoft.EntityFrameworkCore;

namespace TicketingApp.Models
{
    public class TicketDb : DbContext
    {
        public TicketDb(DbContextOptions<TicketDb> options) : base(options) { }
        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<Status> Statuses { get; set; } = null!;
        public DbSet<PointValue> PointValues { get; set; } = null!;
        public DbSet<Sprint> Sprints { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>().HasData(
                new Status { StatusId = "todo", Name = "ToDo" },
                new Status { StatusId = "inprogress", Name = "InProgress" },
                new Status { StatusId = "qa", Name = "QA" },
                new Status { StatusId = "done", Name = "Done" });

            modelBuilder.Entity<PointValue>().HasData(
                new PointValue { PointValueId = 1, Name = "Small Story" },
                new PointValue { PointValueId = 2, Name = "Med Story" },
                new PointValue { PointValueId = 3, Name = "Large Story" },
                new PointValue { PointValueId = 4, Name = "XLarge Story" });

            modelBuilder.Entity<Sprint>().HasData(
                new Sprint(1, DateTime.Now, DateTime.Now.AddDays(14))
            );

            // Seed Ticket entities with SprintId filled in
            modelBuilder.Entity<Ticket>().HasData(
                new Ticket { TicketId = "debugapp", Name = "Debug Application", StatusId = "ToDo", PointValueId = 1, SprintId = 1 },
                new Ticket { TicketId = "loginissue", Name = "Login Issue", StatusId = "InProgress", PointValueId = 3, SprintId = 1 },
                new Ticket { TicketId = "resetuser", Name = "Reset User", StatusId = "Done", PointValueId = 2, SprintId = 1 },
                new Ticket { TicketId = "passwordchange", Name = "Password Change", StatusId = "QA", PointValueId = 4, SprintId = 1 }
            );

            // Configure relationships
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.PointValue)
                .WithMany()
                .HasForeignKey(t => t.PointValueId);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Sprint)
                .WithMany()
                .HasForeignKey(t => t.SprintId);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Status)
                .WithMany()
                .HasForeignKey(t => t.StatusId);
        }
    }
}

