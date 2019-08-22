using Microsoft.EntityFrameworkCore;

namespace SomeReallyComplexProject.Integration.Persistence
{
    public class IntegrationDbContext : DbContext
    {
        public DbSet<IntegrationEventRecord> IntegrationEvents { get; set; }

        public IntegrationDbContext(DbContextOptions<IntegrationDbContext> options) : base(options)
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<IntegrationEventRecord>();

            entity.ToTable("IntegrationEvents");
            entity.HasKey(e => e.EventId);

            entity.Property(e => e.Sender).HasMaxLength(50).IsRequired();
            entity.Property(e => e.EventName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.EventData).IsRequired();
        }
    }
}
