using System;

namespace SomeReallyComplexProject.Core.Integration
{
    public abstract class IntegrationEvent
    {
        public Guid EventId { get; }

        public DateTime Timestamp { get; }

        public IntegrationEvent()
        {
            EventId = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
        }
    }
}
