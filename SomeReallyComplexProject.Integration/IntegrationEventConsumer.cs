using MassTransit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SomeReallyComplexProject.Core.Integration;
using SomeReallyComplexProject.Integration.Persistence;
using System;
using System.Threading.Tasks;

namespace SomeReallyComplexProject.Integration
{
    public class IntegrationEventConsumer : IConsumer<IntegrationEventRecord>
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IEventsRepository eventsRepository;
        private readonly ISubscriptionsManager subscriptionsManager;

        public IntegrationEventConsumer(
            IServiceProvider serviceProvider,
            IEventsRepository eventsRepository,
            ISubscriptionsManager subscriptionsManager)
        {
            this.serviceProvider = serviceProvider;
            this.eventsRepository = eventsRepository;
            this.subscriptionsManager = subscriptionsManager;
        }

        public async Task Consume(ConsumeContext<IntegrationEventRecord> context)
        {
            if (await eventsRepository.IsHandled(context.Message.EventId))
                return;

            await ProcessIntegrationEvent(context.Message);
        }

        private async Task ProcessIntegrationEvent(IntegrationEventRecord eventRecord)
        {
            var eventName = eventRecord.EventName;
            var eventData = eventRecord.EventData;

            foreach (var subscription in subscriptionsManager.GetHandlersForEvent(eventName))
            {
                if (subscription.IsDynamic)
                {
                    dynamic eventPayload = JObject.Parse(eventData);
                    var handler = serviceProvider.GetService(subscription.HandlerType);
                    await ((IDynamicIntegrationEventHandler)handler).Handle(eventPayload);
                }
                else
                {
                    var eventType = subscriptionsManager.GetEventTypeByName(eventName);
                    var eventPayload = JsonConvert.DeserializeObject(eventData, eventType);

                    var eventHandler = serviceProvider.GetService(subscription.HandlerType);
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                    await (Task)concreteType.GetMethod("Handle").Invoke(eventHandler, new object[] { eventPayload });
                }
            }

            eventRecord.MarkAsHandled();
            await eventsRepository.Track(eventRecord);
        }
    }
}
