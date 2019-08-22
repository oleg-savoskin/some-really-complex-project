using SomeReallyComplexProject.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.Core.Persistence
{
    public interface IDomainEventsLogService
    {
        Task LogEventsAsync(Guid correlationId, int iteration, IEnumerable<DomainEvent> events);

        Task MarkHandledAsync(Guid correlationId, IEnumerable<Guid> eventIds);
    }
}
