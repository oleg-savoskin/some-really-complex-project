using MediatR;
using Microsoft.EntityFrameworkCore;
using SomeReallyComplexProject.Core.Domain;
using SomeReallyComplexProject.Core.Domain.Events;
using SomeReallyComplexProject.Core.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.EntityFramework
{
    public abstract class EFContext : DbContext
    {
        protected IMediator Mediator { get; }

        protected IDomainEventsLogService DomainEventsLog { get; }

        protected IEventualConsistencyService EventualConsistencyService { get; }

        protected EFContext(
            DbContextOptions options,
            IMediator mediator,
            IDomainEventsLogService domainEventsLog,
            IEventualConsistencyService eventualConsistencyService) : base(options)
        {
            Mediator = mediator;
            DomainEventsLog = domainEventsLog;
            EventualConsistencyService = eventualConsistencyService;
        }

        public async Task SaveEntitiesAsync(Guid? correlationId, int? iteration, bool publishEvents, CancellationToken cancellationToken)
        {
            var transactionId = correlationId ?? Guid.NewGuid();
            var currentIteration = (iteration ?? 0) + 1;

            try
            {
                if (publishEvents)
                    await DispatchDomainEventsAsync(transactionId, currentIteration);

                await base.SaveChangesAsync(cancellationToken);
            }
            finally
            {
                if (currentIteration == 1)
                    await EventualConsistencyService.EnsureLogicalTransactionCompletedAsync(transactionId);
            }
        }

        private async Task DispatchDomainEventsAsync(Guid correlationId, int iteration)
        {
            var domainEntities = ChangeTracker.Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            await DomainEventsLog
                .LogEventsAsync(correlationId, iteration, domainEvents);

            await Task.WhenAll(domainEvents.Select((e) =>
            {
                var wrapped = WrapEvent(correlationId, iteration, e);
                return Mediator.Publish(wrapped);
            }));

            await DomainEventsLog
                .MarkHandledAsync(correlationId, domainEvents.Select(e => e.EventId));
        }

        private INotification WrapEvent(Guid correlationId, int iteration, DomainEvent @event)
        {
            object[] parameters = { correlationId, iteration, @event, false };
            var wrapperType = typeof(EventWrapper<>).MakeGenericType(@event.GetType());
            return (INotification)Activator.CreateInstance(wrapperType, parameters);
        }
    }
}
