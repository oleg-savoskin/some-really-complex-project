using Newtonsoft.Json;
using SomeReallyComplexProject.Core.Domain.Events;
using System;

namespace SomeReallyComplexProject.EntityFramework.DomainEvents
{
    public class DomainEventRecord
    {
        public Guid EventId { get; private set; }

        public Guid CorrelationId { get; private set; }

        public int Iteration { get; private set; }

        public string EventName { get; private set; }

        public string EventData { get; private set; }

        public bool IsHandled { get; private set; }

        public bool IsUndone { get; private set; }

        public DateTime DateCreated { get; private set; }

        public void MarkHandled() => IsHandled = true;

        public void MarkUndone() => IsUndone = true;

        public DomainEventRecord(
            int iteration,
            Guid correlationId,
            DomainEvent domainEvent)
        {
            EventId = domainEvent.EventId;
            EventName = domainEvent.GetType().Name;
            EventData = JsonConvert.SerializeObject(domainEvent);
            DateCreated = domainEvent.Timestamp;
            CorrelationId = correlationId;
            Iteration = iteration;
        }

        private DomainEventRecord() { }
    }
}
