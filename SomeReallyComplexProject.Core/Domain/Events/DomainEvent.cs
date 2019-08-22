using System;

namespace SomeReallyComplexProject.Core.Domain.Events
{
    public abstract class DomainEvent
    {
        public Guid EventId { get; }

        public DateTime Timestamp { get; }

        public DomainEvent()
        {
            EventId = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
        }
    }
}
