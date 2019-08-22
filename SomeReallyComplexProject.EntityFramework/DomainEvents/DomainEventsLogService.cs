using SomeReallyComplexProject.Core.Domain.Events;
using SomeReallyComplexProject.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.EntityFramework.DomainEvents
{
    public class DomainEventsLogService : IDomainEventsLogService
    {
        private readonly DomainEventsDbContext context;

        public DomainEventsLogService(DomainEventsDbContext context)
        {
            this.context = context;
        }

        public Task LogEventsAsync(Guid correlationId, int iteration, IEnumerable<DomainEvent> events)
        {
            context.DomainEvents.AddRange(
                events.Select(@event => new DomainEventRecord(iteration, correlationId, @event))
            );

            return context.SaveChangesAsync();
        }

        public Task MarkHandledAsync(Guid correlationId, IEnumerable<Guid> eventIds)
        {
            var records = from record in context.DomainEvents
                          where record.CorrelationId == correlationId && eventIds.Contains(record.EventId)
                          select record;

            records.ToList().ForEach(record => record.MarkHandled());
            return context.SaveChangesAsync();
        }
    }
}
