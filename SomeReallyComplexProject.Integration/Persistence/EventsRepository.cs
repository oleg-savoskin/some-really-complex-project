using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.Integration.Persistence
{
    public class EventsRepository : IEventsRepository
    {
        private readonly IntegrationDbContext context;

        public EventsRepository(IntegrationDbContext context)
        {
            this.context = context;
        }

        public Task Track(IntegrationEventRecord integrationEvent)
        {
            var entity = context.IntegrationEvents.Find(integrationEvent.EventId);

            if (entity != null)
            {
                context.Entry(entity).State = EntityState.Detached;
            }

            context.Attach(integrationEvent);
            context.Entry(integrationEvent).State = (
                entity is null ? EntityState.Added : EntityState.Modified
            );

            return context.SaveChangesAsync();
        }

        public Task<IEnumerable<IntegrationEventRecord>> GetPending()
        {
            var events = context.IntegrationEvents.Where(e => e.DatePublished == null);
            return Task.FromResult<IEnumerable<IntegrationEventRecord>>(events.ToList());
        }

        public Task<bool> IsHandled(Guid eventID)
        {
            var entity = context.IntegrationEvents.Find(eventID);
            return Task.FromResult(entity?.DateHandled != null);
        }
    }
}
