using Microsoft.EntityFrameworkCore;

namespace SomeReallyComplexProject.EntityFramework.DomainEvents
{
    public class DomainEventsDbContext : DbContext
    {
        public DbSet<DomainEventRecord> DomainEvents { get; set; }

        public DomainEventsDbContext(DbContextOptions<DomainEventsDbContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<DomainEventRecord>();

            entity.ToTable("DomainEvents");
            entity.HasKey(e => e.EventId);

            entity.Property(e => e.EventName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.EventData).IsRequired();
        }
    }
}
