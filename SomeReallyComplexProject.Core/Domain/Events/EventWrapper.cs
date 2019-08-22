using MediatR;
using System;

namespace SomeReallyComplexProject.Core.Domain.Events
{
    public class EventWrapper<T> : INotification where T : DomainEvent
    {
        public Guid CorrelationId { get; }

        public int Iteration { get; }

        public T DomainEvent { get; }

        public bool IsRollback { get; }

        public EventWrapper(Guid correlationId, int iteration, T domainEvent, bool isRollback)
        {
            CorrelationId = correlationId;
            Iteration = iteration;
            DomainEvent = domainEvent;
            IsRollback = isRollback;
        }
    }
}
