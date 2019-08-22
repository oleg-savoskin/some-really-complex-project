using MediatR;
using Microsoft.EntityFrameworkCore;
using SomeReallyComplexProject.Core.Persistence;
using SomeReallyComplexProject.EntityFramework;
using SomeReallyComplexProject.ServiceOne.Domain.DomainModel.UserAggregate;
using SomeReallyComplexProject.ServiceOne.Domain.Infrastructure.Configurations;

namespace SomeReallyComplexProject.ServiceOne.Domain.Infrastructure
{
    public class ServiceOneContext : EFContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }

        public ServiceOneContext(
            DbContextOptions<ServiceOneContext> options,
            IMediator mediator,
            IDomainEventsLogService domainEventsLog,
            IEventualConsistencyService eventualConsistencyService)
            : base(options, mediator, domainEventsLog, eventualConsistencyService) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserGroupConfiguration());
        }
    }
}
