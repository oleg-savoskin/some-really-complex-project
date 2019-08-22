using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.Core.Domain.Events
{
    public abstract class DomainEventHandler<T> : INotificationHandler<EventWrapper<T>> where T : DomainEvent
    {
        public Task Handle(EventWrapper<T> notification, CancellationToken cancellationToken)
        {
            return !notification.IsRollback ?
                Do(notification.CorrelationId, notification.Iteration, notification.DomainEvent) :
                Undo(notification.CorrelationId, notification.Iteration, notification.DomainEvent);
        }

        public abstract Task Do(Guid correlationId, int iteration, T domainEvent);

        public virtual Task Undo(Guid correlationId, int iteration, T domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}
