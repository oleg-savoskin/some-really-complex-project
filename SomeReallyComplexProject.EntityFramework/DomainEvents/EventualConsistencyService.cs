using MediatR;
using Newtonsoft.Json;
using SomeReallyComplexProject.Core.Domain.Events;
using SomeReallyComplexProject.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.EntityFramework.DomainEvents
{
    public class EventualConsistencyService : IEventualConsistencyService
    {
        private int jobsInProgress = 0;

        private readonly IMediator mediator;
        private readonly Type[] knownEvents;
        private readonly DomainEventsDbContext context;
        private readonly Queue<Guid> transactionsQueue;

        public EventualConsistencyService(DomainEventsDbContext context, IMediator mediator, Type[] knownEvents)
        {
            this.context = context;
            this.mediator = mediator;
            this.knownEvents = knownEvents;
            transactionsQueue = new Queue<Guid>();
        }

        public Task EnsureLogicalTransactionCompletedAsync(Guid correlationId)
        {
            if (Interlocked.CompareExchange(ref jobsInProgress, 0, 0) == 0)
                return EnsureConsistencyAsync(correlationId);

            lock (transactionsQueue)
                transactionsQueue.Enqueue(correlationId);

            return Task.CompletedTask;
        }

        private async Task EnsureConsistencyAsync(Guid correlationId)
        {
            Interlocked.Increment(ref jobsInProgress);

            try
            {
                await EnsureTasksSucceded(correlationId);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            finally
            {
                Interlocked.Decrement(ref jobsInProgress);
            }

            Guid nextCorrelationId;
            lock (transactionsQueue)
                transactionsQueue.TryDequeue(out nextCorrelationId);

            if (nextCorrelationId != Guid.Empty)
                await EnsureConsistencyAsync(nextCorrelationId);
        }

        private async Task EnsureTasksSucceded(Guid correlationId)
        {
            var records = from record in context.DomainEvents
                          where record.CorrelationId == correlationId
                          select record;

            if (records.Any(e => !e.IsHandled))
            {
                var iterations = from record in records
                                 group record by record.Iteration into iteration
                                 orderby iteration.Key descending
                                 select iteration;

                foreach (var iteration in iterations)
                foreach (var record in iteration)
                {
                    await PublishRollbackEvent(record);
                    await MarkEventUndone(record);
                }
            }
        }

        private async Task PublishRollbackEvent(DomainEventRecord record)
        {
            var eventType = knownEvents.SingleOrDefault(e => e.Name == record.EventName);

            if (eventType is null)
                throw new InvalidOperationException($"Unknown event: {record.EventName}");

            var domainEvent = JsonConvert.DeserializeObject(record.EventData, eventType);
            object[] args = { record.CorrelationId, record.Iteration, domainEvent, true };

            var wrapperType = typeof(EventWrapper<>).MakeGenericType(eventType);
            var notification = Activator.CreateInstance(wrapperType, args);

            await mediator.Publish(notification);
        }

        private Task MarkEventUndone(DomainEventRecord record)
        {
            record.MarkUndone();
            return context.SaveChangesAsync();
        }
    }
}
