using Microsoft.EntityFrameworkCore;
using starter_code.Models;

namespace starter_code.Data
{
    public class EventAppDbContext : DbContext
    {
        public EventAppDbContext(DbContextOptions<EventAppDbContext> options)
            : base(options) { }

        public DbSet<Message> Messages => Set<Message>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Message>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
